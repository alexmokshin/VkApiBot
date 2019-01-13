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
        public ulong ApplicationId { get; set; } 
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

        //Чтобы увеличить уровень безопасности личных данных в приложении, будем принимать строку с паролем в качестве защищенной
        //и возвращать ее
        private SecureString VkPassword()
        {
            SecureString secPasswd = new SecureString();
            ConsoleKeyInfo keyInfo;
            Console.Write("Enter password: ");
            do
            {
                keyInfo = Console.ReadKey(true);
                if (IS_VALID_KEY_RANGE((int)keyInfo.Key))
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
            
            return secPasswd;
            
        }

        public static Predicate<int> IS_VALID_KEY_RANGE = delegate (int t) { return (t >= 10 && t <= 300 && t!=13); };
        //Поскольку, API не умеет работать с защищенными строками, придется конвертировать ее назад в string
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
        //Создадим функцию, которая будет возвращать поля, необходимые для аутентификации
        private VkAuthParams GetApiAuthParams(string username)
        {
            const ulong APPLICATION_ID = 6810122;
            var auth_params = new VkAuthParams();
            if (!String.IsNullOrWhiteSpace(username))
            {
                auth_params.ApplicationId = APPLICATION_ID;
                auth_params.Login = username;
                auth_params.Password = SecureStringToString(VkPassword());
                auth_params.Settings = Settings.Wall;
                auth_params.TwoFactorAuthorization = () =>
                {
                    Console.WriteLine("Enter code, if you enable double-auth security. If you dont'use it-press Enter");
                    return Console.ReadLine();
                };

                return auth_params;
            }
            else
                throw new ArgumentNullException(username, "Enter login to authentication");
            
        }
        //Перепишем метод авторизации, поскольку дефлотный не учитывает двух-факторную аутентификацию, да и паролем будет пользоваться безопаснее.
        //согласно тестам, прочитать его нельзя.
        public VkApi Authorize(string username)
        {
            var API = new VkApi();
            try
            {
                API.Authorize(GetApiAuthParams(username));
            }
            catch (VkNet.Exception.VkApiException)
            {
                Console.WriteLine("Логин или пароль введен неправильно. Пожалуйста, попробуйте еще раз");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return API;
        }
    }
}
