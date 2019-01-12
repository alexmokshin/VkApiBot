using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;

namespace VkApiBotTest
{
    interface IPostAdministrate<T> where T:class
    {
        List<T> GetPostsFromUsername(VkApi _api, string username, int count);
        void SetWallPostToUsername(VkApi _api, string message, string postOwner,long toUsername = 0);
    }
}
