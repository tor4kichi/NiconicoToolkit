﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text.Json;
using NiconicoToolkit.User;

#if WINDOWS_UWP
using Windows.Web.Http;
using Windows.Web.Http.Headers;
#else
using System.Net;
using System.Net.Http;
#endif


namespace NiconicoToolkit.Account
{
    public sealed class NiconicoTwoFactorAuthEventArgs
    {
        private readonly AccountClient _account;

        public  Uri Location { get; }

        internal NiconicoTwoFactorAuthEventArgs(AccountClient account, Uri location)
        {
            _account = account;
            Location = location;
        }

        public async Task<NiconicoSessionStatus> MfaAsync(string code, bool isTrustedDevice, string deviceName)
        {
            return await _account.MfaAsync(Location, code, isTrustedDevice, deviceName);
        }
    }

    public sealed class NiconicoLoggedInEventArgs
    {
        public NiconicoLoggedInEventArgs(UserId userId, bool isPremiumAccount)
        {
            UserId = userId;
            IsPremiumAccount = isPremiumAccount;
        }

        public UserId UserId { get; }
        public bool IsPremiumAccount { get; }
    }

    public sealed class AccountClient
    {
        private readonly NiconicoContext _context;

        private const string MailTelName = "mail_tel";
        private const string PasswordName = "password";


        private const string NicoVideoTopPage_SignInCheckUrl = "https://www.nicovideo.jp";
        private const string LoginPageUrl = "https://www.nicovideo.jp/login";
        private const string LogInApiUrl = "https://account.nicovideo.jp/api/v1/login";
        private const string TwoFactorAuthSite = "https://account.nicovideo.jp/mfa";

        private const string XNiconicoId = "x-niconico-id";
        private const string XNiconicoAuthflag = "x-niconico-authflag";

        private const string LogOffUrl = "https://secure.nicovideo.jp/secure/logout";

        internal AccountClient(NiconicoContext context)
        {
            _context = context;
        }

        public event EventHandler<NiconicoLoggedInEventArgs> LoggedIn;
        public event EventHandler LogInFailed;
        public event EventHandler<NiconicoTwoFactorAuthEventArgs> RequireTwoFactorAuth;
        public event EventHandler LoggedOut;


        public async Task<NiconicoSessionStatus> CheckSessionStatusAsync()
        {
            using var res = await _context.GetAsync(NicoVideoTopPage_SignInCheckUrl);
            return __IsSignedInAsync_Internal(res);
        }

        public async Task<NiconicoSessionStatus> SignOutAsync()
        {
            using var res = await _context.GetAsync(LogOffUrl);
            LoggedOut?.Invoke(this, EventArgs.Empty);
            return __IsSignedInAsync_Internal(res);
        }

        public async Task<(NiconicoSessionStatus status, NiconicoAccountAuthority authority, UserId userId)> GetCurrentSessionAsync()
        {
            using var res = await _context.GetAsync(LoginPageUrl);
            var (authority, userId) = __GetAccountAuthority_Internal(res);
            var status = __IsSignedInAsync_Internal(res);
            return (status, authority, userId);
        }

        public async Task<(NiconicoSessionStatus status, NiconicoAccountAuthority authority, UserId userId)> SignInAsync(MailAndPasswordAuthToken authToken, CancellationToken ct = default)
        {
            var dict = new Dictionary<string, string>()
            {
                { MailTelName, authToken.Mail },
                { PasswordName, authToken.Password }
            };

            {
                using var res = await _context.GetAsync(LoginPageUrl);
                if (!res.IsSuccessStatusCode)
                {
                    return (NiconicoSessionStatus.ServiceUnavailable, NiconicoAccountAuthority.NotSignedIn, default);
                }

                var (signInStatus, _) = __GetAccountAuthority_Internal(res);
                if (signInStatus != NiconicoAccountAuthority.NotSignedIn)
                {
                    try
                    {
                        await SignOutAsync();
                    }
                    catch { }
                    await Task.Delay(100);
                }
            }

            using var loginResultResponse = await _context.PostAsync(LogInApiUrl, dict, ct);

            if (IsRequireTwoFactorAuth(loginResultResponse.RequestMessage))
            {
                RequireTwoFactorAuth?.Invoke(this, new NiconicoTwoFactorAuthEventArgs(this, loginResultResponse.RequestMessage.RequestUri));

                return (NiconicoSessionStatus.RequireTwoFactorAuth, NiconicoAccountAuthority.NotSignedIn, default);
            }
            else
            {
                var result = __GetAccountAuthority_Internal(loginResultResponse);
                if (result.authority == NiconicoAccountAuthority.NotSignedIn)
                {
                    LogInFailed?.Invoke(this, EventArgs.Empty);
                    return (NiconicoSessionStatus.Failed, result.authority, default);
                }
                else
                {
                    LoggedIn?.Invoke(this, new NiconicoLoggedInEventArgs(result.userId, result.authority == NiconicoAccountAuthority.Premium));
                    return (NiconicoSessionStatus.Success, result.authority, result.userId);
                }
            }
        }

#if WINDOWS_UWP
        private static bool IsRequireTwoFactorAuth(HttpRequestMessage message)
        {
            return message.RequestUri.OriginalString.StartsWith(TwoFactorAuthSite);
        }
#else
        private static bool IsRequireTwoFactorAuth(HttpRequestMessage message)
        {
            return message.RequestUri.OriginalString.StartsWith(TwoFactorAuthSite);
        }
#endif

        private NiconicoSessionStatus __IsSignedInAsync_Internal(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
#if WINDOWS_UWP
                if (response.Headers.TryGetValue(XNiconicoAuthflag, out var flags))
                {
                    var authFlag = flags.ToUInt();
                    var auth = (NiconicoAccountAuthority)authFlag;
                    return auth != NiconicoAccountAuthority.NotSignedIn ? NiconicoSessionStatus.Success : NiconicoSessionStatus.Failed;
                }
#else
                if (response.Headers.TryGetValues(XNiconicoAuthflag, out var flags))
                {
                    var authFlag = flags.First().ToUInt();
                    var auth = (NiconicoAccountAuthority)authFlag;
                    return auth != NiconicoAccountAuthority.NotSignedIn ? NiconicoSessionStatus.Success : NiconicoSessionStatus.Failed;
                }
#endif
            }
            else if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                return NiconicoSessionStatus.ServiceUnavailable;
            }

            return NiconicoSessionStatus.Failed;
        }

        private (NiconicoAccountAuthority authority, UserId userId) __GetAccountAuthority_Internal(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
#if WINDOWS_UWP
                if (response.Headers.TryGetValue(XNiconicoAuthflag, out var flags))
                {
                    var authFlag = flags.ToUInt();
                    var auth = (NiconicoAccountAuthority)authFlag;
                    if (auth != NiconicoAccountAuthority.NotSignedIn)
                    {
                        if (response.Headers.TryGetValue(XNiconicoId, out var userId))
                        {
                            return (auth, new UserId(userId));
                        }
                    }
                }
#else
                if (response.Headers.TryGetValues(XNiconicoAuthflag, out var flags))
                {
                    var authFlag = flags.First().ToUInt();
                    var auth = (NiconicoAccountAuthority)authFlag;
                    if (auth != NiconicoAccountAuthority.NotSignedIn)
                    {
                        if (response.Headers.TryGetValues(XNiconicoId, out var userIds))
                        {
                            var userId = new UserId(userIds.First().ToUInt());
                            return (auth, userId);
                        }
                    }
                }
#endif
            }

            return (NiconicoAccountAuthority.NotSignedIn, default);
        }



        public async Task<NiconicoSessionStatus> MfaAsync(Uri location, string code, bool isTrustedDevice, string deviceName)
        {
#if WINDOWS_UWP
            using var content = new HttpFormUrlEncodedContent(new Dictionary<string, string>()
#else
            using var content = new FormUrlEncodedContent(new Dictionary<string, string>()
#endif
            {
                { "otp", code },
                { "loginBtn", "ログイン" },
                { "is_mfa_trusted_device", isTrustedDevice ? "true" : "false" },
                { "device_name", deviceName }
            });


#if WINDOWS_UWP
            void HeaderFiller(HttpRequestHeaderCollection headers)
#else
            void HeaderFiller(HttpRequestHeaders headers)
#endif
            {
//                headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; ServiceUI 11) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299");
                headers.Add("Referer", location.OriginalString);
                headers.Add("Origin", "https://account.nicovideo.jp");
                headers.Add("Upgrade-Insecure-Requests", "1");
                headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
            }

            using var res = await _context.SendAsync(HttpMethod.Post, location, content, HeaderFiller, HttpCompletionOption.ResponseHeadersRead);
            var status = __IsSignedInAsync_Internal(res); 
            if (status == NiconicoSessionStatus.Success)
            {
                var info = __GetAccountAuthority_Internal(res);
                LoggedIn?.Invoke(this, new NiconicoLoggedInEventArgs(info.userId, info.authority == NiconicoAccountAuthority.Premium));
            }

            return status;
        }
    }
}
