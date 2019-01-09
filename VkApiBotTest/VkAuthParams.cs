using System;
using System.Security;
using VkNet;
using VkNet.Abstractions.Authorization;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace VkApiBotTest
{
    class VkAuthParams : IApiAuthParams
    {
        public ulong ApplicationId { get; set; } = 6810122;
        public string Login { get; set; }
        public string Password { get; set; }
        public Settings Settings { get; set; } = Settings.Wall;
        public Func<string> TwoFactorAuthorization { get; set; }
        public string AccessToken { get; set; }
        public int TokenExpireTime { get; set; }
        public long UserId { get; set; }
        public long? CaptchaSid { get; set; }
        public string CaptchaKey { get; set; }
        public string Host { get; set; }
        public int? Port { get; set; }
        public string ProxyLogin { get; set; }
        public string ProxyPassword { get; set; }
        public string Phone { get; set; }

        public static SecureString vkPassword()
        {
            SecureString secPasswd = new SecureString();
            ConsoleKeyInfo keyInfo;
            Console.Write("Enter password: ");
            do
            {
                keyInfo = Console.ReadKey(true);
                if (((int)keyInfo.Key) >= 65 && ((int)keyInfo.Key <= 90))
                {
                    secPasswd.AppendChar(keyInfo.KeyChar);
                    Console.Write("*");
                }
               
            }
            while (keyInfo.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return secPasswd;
            
        }

        
    }
}
