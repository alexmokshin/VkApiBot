using System;
using System.Collections.Generic;
using System.Linq;
using VkNet.Model.Attachments;
using VkNet;

namespace VkApiBotTest
{
    class PostAdministration : IPostAdministrate<Post>
    {
        public long PostOwner {get;set;}
        //функция, в которую мы будем возращать посты пользователя. Условия - чтобы посты были, и чтобы в постах был текст
        public List<Post> GetPostsFromUsername(VkApi _api, string username, int count=5)
        {
            if (username!=null)
            {
                username = NormalizeUsernameString(username);
                try
                {
                    var getPosts = _api.Wall.Get(new VkNet.Model.RequestParams.WallGetParams
                    {
                        Domain = username,
                        Count = (ulong)count
                    });
                    List<Post> userPosts = getPosts.WallPosts.Where(item => !String.IsNullOrEmpty(item.Text)).ToList();
                    if (userPosts.Count == 0)
                        throw new Exception("У пользователя нет постов с текстом, для представления");
                    //Здесь будем айди владельца поста. По хорошему - иметь бы профиль пользователя, но пока времени нет.
                    PostOwner = getPosts.WallPosts[0].OwnerId.Value;
                    return userPosts;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
                {
                throw new ArgumentNullException(username, "Введите имя пользователя, чьи посты необходимо проверить");
                }
            return null;
        }

        public void SetWallPostToUsername(VkApi _api, string message,  long toUsername = 0)
        {
            long newPostId;
            string postMessage = String.Format("@id{0}, статистика для последних 5 постов: {1}", PostOwner, message);
            if (toUsername == 0)
            {
                newPostId = _api.Wall.Post(new VkNet.Model.RequestParams.WallPostParams
                {
                    Message = postMessage
                });
            }
            else
            {
                newPostId = _api.Wall.Post(new VkNet.Model.RequestParams.WallPostParams
                {
                    OwnerId = toUsername,
                    Message = postMessage
                });
            }
            if (newPostId > 0)
                Console.WriteLine("Пост успешно размещен. Id поста: {0}", (int)newPostId);
            else
                Console.WriteLine("Пост не размешен");
            
        }

        string NormalizeUsernameString(string username)
        {
            string temp;
            if (username.StartsWith("public"))
            {
                temp = username.Replace("public", "-");
                return temp;
            }
            else if (username.StartsWith("-"))
            {
                temp = username.Replace("-", "public");
                return temp;
            }
            return username;
        }
    }
}
