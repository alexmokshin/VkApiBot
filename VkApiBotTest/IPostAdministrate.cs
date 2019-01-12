using System.Collections.Generic;
using VkNet;

namespace VkApiBotTest
{
    interface IPostAdministrate<T> where T:class
    {
        long PostOwner { get; set; }
        List<T> GetPostsFromUsername(VkApi _api, string username, int count);
        void SetWallPostToUsername(VkApi _api, string message,long toUsername = 0);
    }
}
