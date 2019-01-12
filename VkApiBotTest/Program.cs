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
        
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            GetPostsFromVkViaApi();
            Console.ReadKey();
        }
        static void GetPostsFromVkViaApi()
        {
            String FullPostText = null;
            PostAdministration administration = new PostAdministration();
            var vkAuth = new VkAuthParams();
            Console.WriteLine("Enter login: ");
            string login = Console.ReadLine();
            var vkApi = vkAuth.Authorize(login);
            if (vkApi.IsAuthorized)
            {
                Console.WriteLine("Enter id or username, which posts need to calcualte");
                string postOwner = Console.ReadLine();
                List<VkNet.Model.Attachments.Post> posts = administration.GetPostsFromUsername(vkApi, postOwner);
                // TODO: Вынести параметры запрашиваемого поста в отдельный класс. Реализовать в классе проверку на ввод домена.
                Dictionary<char, double> massSymbPairs = new Dictionary<char, double>();
                
                    
                posts.ForEach(item => FullPostText += item.Text.ToUpper());
                foreach (var t in FullPostText.ToUpper())
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
                //TODO: Реализовать класс, который будет обрабатывать статистику из постов, возвращать JSON. Постараться абстрагироваться от листа с текстом. Пусть принимает весь класс POST и его обрабатывает. В частности, выдергивает текст, и считает статистику
                string json = JsonConvert.SerializeObject(massSymbPairs);
                administration.SetWallPostToUsername(vkApi, json,posts[0].OwnerId.ToString());
               
               
            }
            
            
        }
        static async Task<string> GetPostFromVk(ApiVkBotSettings apiVk)
        {
            HttpResponseMessage response = await client.GetAsync("https://api.vk.com/method/wall.get?domain=doysproject&count=5&filter=all&v=5.92&access_token="+apiVk.secret_service_uuid);
            
            string source = null;
            if (response.IsSuccessStatusCode)
                source = await response.Content.ReadAsStringAsync();
            return source ;
        }

    }
}
