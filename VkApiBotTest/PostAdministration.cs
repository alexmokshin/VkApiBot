﻿using System;
using System.Collections.Generic;
using System.Linq;
using VkNet.Model.Attachments;
using VkNet;

namespace VkApiBotTest
{
    class PostAdministration : IPostAdministrate<Post>
    {
        public string UserDomain { get; set; }
        public long UserOwnerId { get; set; }
        public long PostOwner {get;set;}
        //функция, в которую мы будем возращать посты пользователя. Условия - чтобы посты были, и чтобы в постах был текст
        public List<Post> GetPostsFromUsername(VkApi _api, string inputUsername, int count=5)
        {

            try
            {

                //поскольку API некорректно воспринимает полное id паблика, из которого надо брать посты, будем приводить введенное пользователем имя
                //в корректный вид
                UserDomain = NormalizeUsernameString(inputUsername);

                var getPosts = _api.Wall.Get(new VkNet.Model.RequestParams.WallGetParams
                {   //у API множество вариаций запроса, для выполнения задачи достаточно использовать Domain или OwnerId
                    //у поля OwnerId приоритет выше, но для упрощенных ID пользователя, будем использовать Domain
                    Domain = UserDomain,
                    OwnerId = UserOwnerId,
                    Count = (ulong)count
                });

                //Соберем посты в коллекцию, и отфильтруем по тексту. Для анализа, не нужны посты без текста.
                List<Post> user_posts = getPosts.WallPosts.Where(item => !String.IsNullOrEmpty(item.Text)).ToList();
                if (user_posts.Count != 0)
                {
                    PostOwner = getPosts.WallPosts[0].OwnerId.Value;
                    return user_posts;
                    
                }
                //Здесь будем айди владельца поста. По хорошему - иметь бы профиль пользователя, но пока времени нет.
                else
                    throw new Exception("У пользователя нет постов с текстом, для представления");
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
        

        //Метод для создания поста на стене авторизованного пользователя
        //параметр toUsername по умолчанию ноль, поскольку отвечает за то, на чьей стене будет размещен пост.
        public void SetWallPostActivityToUsername(VkApi _api, string message, long toUsername = 0)
        {
            //в лонге будем хранить id поста
            long newPostId = 0;
            //Приведем id пользователя в красивый вид, чтобы JS в Вконтакте отмечал пользователя ссылкой
            string usernameId = NormalizeUsernameString(PostOwner.ToString());
            //Приведем текст поста к определенному формату
            string post_message = String.Format("@{0}, статистика для последних 5 постов: {1}", usernameId, message);
            if (toUsername == 0)
            {
                //если мы не отметили, на чьей стене будем размещать пост, по умолчанию размещаем на стене авторизованного пользователя
                newPostId = _api.Wall.Post(new VkNet.Model.RequestParams.WallPostParams
                {
                    Message = post_message
                });
            }
            else
            {
                //иначе - на стене того, кого укажем
                newPostId = _api.Wall.Post(new VkNet.Model.RequestParams.WallPostParams
                {
                    OwnerId = toUsername,
                    Message = post_message
                });
            }
            //Рапортуем, что пост размещен корректно
            if (newPostId > 0)
                Console.WriteLine("Пост успешно размещен. Id поста: {0}", (int)newPostId);
            else
                Console.WriteLine("Пост не размешен");
            
        }

        //Ввиду разницы того, как формализованно необходимо обращаться к API в ВКонтакте, нарисуем костыль, который будет приводить все к нужному виду.
        string NormalizeUsernameString(string username)
        {
            string temp;
            if (username.StartsWith("public"))
            {
                temp = username.Replace("public", "-");
                UserOwnerId = Convert.ToInt64(temp);
            }
            else if (username.StartsWith("-"))
            {
                temp = username.Replace("-", "public");
            }
            else if (Char.IsDigit(username,0))
            {
                temp = "id" + username;
            }
            else
            {
                temp = username;
            }
            return temp;
        }
    }
}
