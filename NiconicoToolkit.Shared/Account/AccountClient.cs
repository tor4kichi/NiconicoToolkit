using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text.Json;
using NiconicoToolkit.User;
using System.Text.Json.Serialization;


#if WINDOWS_UWP
using Windows.Web.Http;
using Windows.Web.Http.Headers;
#else
using System.Net;
using System.Net.Http;
#endif


namespace NiconicoToolkit.Account;

[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata)]
[JsonSerializable(typeof(LoginAccountUserResponse))]
public sealed partial class AccountUserJsonSourceGenerationContext : JsonSerializerContext
{
}

public sealed class AccountClient
{
    private readonly NiconicoContext _context;
    private readonly JsonSerializerOptions _options;
    public const string LoginPageUrl = "https://account.nicovideo.jp/spa/login/index.html?sec=header_pc&redirect_uri=https%3A%2F%2Fwww.nicovideo.jp%2F";    
    private const string NicoVideoTopPage_SignInCheckUrl = "https://www.nicovideo.jp";

    private const string XNiconicoId = "x-niconico-id";
    private const string XNiconicoAuthflag = "x-niconico-authflag";

    internal AccountClient(NiconicoContext context, JsonSerializerOptions options)
    {
        _context = context;
        _options = new(options)
        {
            TypeInfoResolverChain = { AccountUserJsonSourceGenerationContext.Default }
        };
    }

    public async Task<NiconicoSessionStatus> CheckSessionStatusAsync()
    {
        using var res = await _context.GetAsync(NicoVideoTopPage_SignInCheckUrl);
        return __IsSignedInAsync_Internal(res);
    }

    public async Task<NiconicoSessionStatus> SignOutAsync()
    {
        using var res = await _context.SendAsync(HttpMethod.Delete, "https://api.id.nicovideo.jp/v1/sessions/me");        
        return __IsSignedInAsync_Internal(res);
    }

    public async Task<(NiconicoSessionStatus status, NiconicoAccountAuthority authority, UserId userId)> GetCurrentSessionAsync()
    {
        using var res = await _context.GetAsync(NicoVideoTopPage_SignInCheckUrl);
        var (authority, userId) = __GetAccountAuthority_Internal(res);
        var status = __IsSignedInAsync_Internal(res);
        return (status, authority, userId);
    }

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

    public async Task<LoginAccountUserResponse> GetUserAccountAsync()
    {
        return await _context.GetJsonAsAsync<LoginAccountUserResponse>(
            $"https://account.nicovideo.jp/api/public/v2/user.json", _options);
    }
}

public class Data
{
    [JsonPropertyName("userId")]
    public UserId UserId { get; set; }

    [JsonPropertyName("nickname")]
    public string Nickname { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; }

    [JsonPropertyName("locale")]
    public string Locale { get; set; }

    [JsonPropertyName("area")]
    public string Area { get; set; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; set; }

    [JsonPropertyName("isExplicitlyLoginable")]
    public bool IsExplicitlyLoginable { get; set; }

    [JsonPropertyName("hasPremiumOrStrongerRights")]
    public bool HasPremiumOrStrongerRights { get; set; }

    [JsonPropertyName("hasSuperPremiumOrStrongerRights")]
    public bool HasSuperPremiumOrStrongerRights { get; set; }

    [JsonPropertyName("premium")]
    public Premium Premium { get; set; }

    [JsonPropertyName("icons")]
    public UsersResponse.Icons Icons { get; set; }

    [JsonPropertyName("existence")]
    public Existence Existence { get; set; }
}

public class Existence
{
    [JsonPropertyName("residence")]
    public Residence Residence { get; set; }

    [JsonPropertyName("birthday")]
    public string Birthday { get; set; }

    [JsonPropertyName("sex")]
    public string Sex { get; set; }
}

public class Premium
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}

public class Residence
{
    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("prefecture")]
    public string Prefecture { get; set; }
}

public class LoginAccountUserResponse : ResponseWithMeta
{
    [JsonPropertyName("data")]
    public Data Data { get; set; }
}
