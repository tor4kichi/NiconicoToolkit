using NiconicoToolkit.Account;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
#if WINDOWS_UWP
using Windows.Storage;
#else
#endif

namespace NiconicoToolkit.UWP.Test.Tests
{
    public class AccountInfo
    {
        [JsonPropertyName("mail")]
        public string Mail { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

    }

    public static class AccountTestHelper 
    {
        public const string Site = "https://github.com/tor4kichi/NiconicoToolkit";

        public static async Task<AccountInfo> AccountLoadingAsync()
        {
#if WINDOWS_UWP
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/TestAccount.json"));
            using (var stream = (await file.OpenSequentialReadAsync()).AsStreamForRead())
            {
                return await JsonSerializer.DeserializeAsync<AccountInfo>(stream);
            }
#else
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"TestAccount.json");
            return await JsonSerializer.DeserializeAsync<AccountInfo>(File.OpenRead(path));
#endif
        }

        public static async Task<(NiconicoSessionStatus status, NiconicoAccountAuthority authority, uint userId)> LogInWithTestAccountAsync(NiconicoContext niconicoContext)
        {
            var accountInfo = await AccountLoadingAsync();
            return await niconicoContext.Account.SignInAsync(new MailAndPasswordAuthToken(accountInfo.Mail, accountInfo.Password));
        }

        public static async Task<(NiconicoContext niconicoContext, NiconicoSessionStatus status, NiconicoAccountAuthority authority, uint userId)> CreateNiconicoContextAndLogInWithTestAccountAsync()
        {
            NiconicoContext niconicoContext = new NiconicoContext(Site);
            niconicoContext.SetupDefaultRequestHeaders();
            var accountInfo = await AccountLoadingAsync();
            var res = await niconicoContext.Account.SignInAsync(new MailAndPasswordAuthToken(accountInfo.Mail, accountInfo.Password));
            return (niconicoContext:  niconicoContext, res.status, res.authority, res.userId);
        }
    }
}
