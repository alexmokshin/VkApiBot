using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;
using VkNet.Enums.Filters;
using Newtonsoft.Json;
using System.Security;
using System.Net;

namespace VkApiBotTest
{
    class ApiVkBotSettings
    {
        public int client_id { get; set; } = 6810122;
        public string client_secret { get; set; } = "M0wX926WwJS1nkBAfP5H";
        public string redirect_uri { get; set; } = "https://oauth.vk.com/blank.html";
        public string secret_service_uuid { get; set; } = "d7088ae1d7088ae1d7088ae1dcd76f60ebdd708d7088ae18b3bb4896569f5339e033605";

    }
    
    class Program
    {
        static String FullPostText = null;
        static HttpClient client = new HttpClient();
        static Task<string> vs = null;
        static void Main(string[] args)
        {
            
            List<string> postTexts = new List<string>()
            {
                "Мама мыла раму",
                "Мама мыла пол",
                "Папа страдал хуйней",
                "Брат курил гашиш",
                "А я кодил"
            };
            
            Dictionary<char, double> massSymbPairs = new Dictionary<char, double>();
            //string test = "Ааааааббббвбвовроаждячсзщлмгтколотцуроиккждлвааааадлоьыващштцрориывадлокдль";
            postTexts.ForEach(item => FullPostText += item);
            foreach (var t in FullPostText)
            {
                if (Char.IsLetter(t))
                {
                    if (!massSymbPairs.ContainsKey(t))
                    {
                        double cntSimb = FullPostText.Count(item => (char)item == t);
                        double freq = Math.Round(cntSimb / FullPostText.Length, 3);
                        massSymbPairs.Add(t, freq);
                    }
                }
            }
            foreach (var t in massSymbPairs)
                Console.WriteLine("Символ {0} частность {1}", t.Key, t.Value);
            string json = JsonConvert.SerializeObject(massSymbPairs);
            Console.WriteLine(json);
            //GetPostsFromVkViaApi();
            //GetAuth(null, null);
            Console.ReadKey();
            

        }
        static void GetPostsFromVkViaApi()
        {
            WebClient webClient = new WebClient();
            //string response = webClient.DownloadString()
            var api = new VkApi();
            SecureString ssg = new SecureString();
            //ssg = "Zgd5897jyg%";
            api.Authorize(new ApiAuthParams
            {
                ApplicationId = 6810122,
                Login = "+79827160928",
                Password = "Zgd5897jygg%",
                Settings = Settings.Wall,
                TwoFactorAuthorization = () =>
                {
                    Console.WriteLine("Enter code:");
                    return Console.ReadLine();
                }
            });

            //Console.WriteLine(api.Token);
            var get = api.Wall.Get(new VkNet.Model.RequestParams.WallGetParams
            {
                Domain = "id1",
                Count = 5
            });

            List<VkNet.Model.Attachments.Post> listPosts = get.WallPosts.Where(item => item.Text != "").ToList();
            foreach (var item in listPosts)
                Console.WriteLine(item.Text);
            //Console.WriteLine(get.WallPosts.Where(item => item.Text != "" ).ToString());
            
        }
        static void GetAuth(string username, string password)
        {
            WebClient webClient = new WebClient();
            var t = webClient.DownloadString("http://api.vkontakte.ru/oauth/authorize?client_id=6810122&scope=offline,wall");
           

        }
        static async Task<string> GetPostFromVk(ApiVkBotSettings apiVk)
        {
            HttpResponseMessage response = await client.GetAsync("https://api.vk.com/method/wall.get?domain=doysproject&count=5&filter=all&v=5.92&access_token="+apiVk.secret_service_uuid);
            
            string source = null;
            if (response.IsSuccessStatusCode)
                source = await response.Content.ReadAsStringAsync();
            return source ;
        }
        static async Task<string> PostMessageToVk(ApiVkBotSettings apiVk)
        {
            HttpResponseMessage response = await client.GetAsync("https://api.vk.com/method/wall.post?owner_id=-84599507&message=Тест&v=5.92&access_token=" + apiVk.secret_service_uuid);
            string source = null;
            if (response.IsSuccessStatusCode)
                source = await response.Content.ReadAsStringAsync();
            return source;
        }
    }
}
