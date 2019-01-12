using System;
using System.Collections.Generic;

namespace VkApiBotTest
{
    class Program
    {
        static void Main(string[] args)
        {
            GetPostsFromVkViaApi();
            Console.WriteLine("Нажмите любую кнопку, для завершения работы программы");
            Console.ReadKey();
            Environment.Exit(1);
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
                while (true)
                {
                    try
                    {
                        Console.WriteLine("Enter id or username, which posts need to calcualte");
                        string postOwner = Console.ReadLine();
                        if (postOwner == null || postOwner == "")
                            break;
                        List<VkNet.Model.Attachments.Post> posts = administration.GetPostsFromUsername(vkApi, postOwner);
                        string json = PostTextConversation.JsonPostConversation(posts);
                        //administration.SetWallPostToUsername(vkApi, json);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

    }
}
