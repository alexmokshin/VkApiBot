using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet.Model.Attachments;
using VkNet;

namespace VkApiBotTest
{
    class PostAdministration : IPostAdministrate<Post>
    {
        public List<Post> GetPostsFromUsername(VkApi _api, string username, int count=5)
        {
            if (username!=null)
            {
                try
                {
                    var get = _api.Wall.Get(new VkNet.Model.RequestParams.WallGetParams
                    {
                        Domain = username,
                        Count = (ulong)count
                    });
                    List<Post> posts = get.WallPosts.Where(item => item.Text != "").ToList();
                    return posts;
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

        public void SetWallPostToUsername(VkApi _api, string message, string postOwner, long toUsername = 0)
        {
            string postMessage = String.Format("@id{0}, статистика для последних 5 постов: {1}", postOwner, message);
            if (toUsername == 0)
            {
                var set = _api.Wall.Post(new VkNet.Model.RequestParams.WallPostParams
                {
                    Message = postMessage
                });
            }
            else
            {
                var set = _api.Wall.Post(new VkNet.Model.RequestParams.WallPostParams
                {
                    OwnerId = toUsername,
                    Message = postMessage
                });
            }
        }
    }
}
