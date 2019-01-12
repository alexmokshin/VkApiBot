using System;
using System.Runtime.InteropServices;
using System.Security;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace VkApiBotTest
{
    class VkAuthParams : IApiAuthParams
    {
        #region Properties
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
        #endregion

        private SecureString VkPassword()
        {
            SecureString secPasswd = new SecureString();
            ConsoleKeyInfo keyInfo;
            Console.Write("Enter password: ");
            do
            {
                keyInfo = Console.ReadKey(true);
                if (((int)keyInfo.Key) >= 10 && ((int)keyInfo.Key <= 300))
                {
                    secPasswd.AppendChar(keyInfo.KeyChar);
                    Console.Write("*");
                }
                else if(keyInfo.Key == ConsoleKey.Backspace)
                {
                    Console.Write("\b");
                    secPasswd.RemoveAt(secPasswd.Length - 1);
                    
                }
               
            }
            while (keyInfo.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return secPasswd;
            
        }
        private String SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
        public VkAuthParams GetApiAuthParams(string username)
        {
            var authParams = new VkAuthParams();
            if (username != null)
            {
                authParams.Login = username;
                authParams.Password = SecureStringToString(VkPassword());
                authParams.Settings = Settings.Wall;
                authParams.TwoFactorAuthorization = () =>
                {
                    Console.WriteLine("Enter code, if you enable double-auth security. If you dont'use it - press Enter");
                    return Console.ReadLine();
                };
                return authParams;
            }
            else
                throw new ArgumentNullException(username, "Enter login to authentication");
            
        }
        public VkApi Authorize(string username)
        {
            var api = new VkApi();
            try
            {
                api.Authorize(GetApiAuthParams(username));
            }
            catch (VkNet.Exception.VkApiException)
            {
                Console.WriteLine("Пароль введен неправильно. Пожалуйста, попробуйте еще раз");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return api;
        }
    }
}
