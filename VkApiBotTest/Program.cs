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
    class Program
    {
        static void Main(string[] args)
        {
            GetPostsFromVkViaApi();
            Console.ReadKey();
        }
        static void GetPostsFromVkViaApi()
        {
            
            PostAdministration administration = new PostAdministration();
            var vkAuth = new VkAuthParams();
            Console.WriteLine("Enter login: ");
            string login = Console.ReadLine();
            var vkApi = vkAuth.Authorize(login);
            if (vkApi.IsAuthorized)
            {
                try
                {
                    Console.WriteLine("Enter id or username, which posts need to calcualte");
                    string postOwner = Console.ReadLine();
                    List<VkNet.Model.Attachments.Post> posts = administration.GetPostsFromUsername(vkApi, postOwner);
                    string json = PostTextConversation.JsonPostConversation(posts);
                    administration.SetWallPostToUsername(vkApi, json, posts[0].OwnerId.ToString());
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

    }
}
