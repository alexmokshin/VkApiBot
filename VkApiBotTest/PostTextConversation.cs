using System;
using System.Collections.Generic;
using System.Linq;
using VkNet.Model.Attachments;
using Newtonsoft.Json;

namespace VkApiBotTest
{
    class PostTextConversation
    {
        //Данный метод будет возвращать строку JSON из листа с постами
        public static string JsonPostConversation(List<Post> list)
        {
            //В ходе анализа, чтобы анализировать частотность букв, уместее все приводить к одной строке, и уже на ее основании проводить расчет
            //Идея в том, что пока размер строки не превышает 4 Гб, расчет будет идти довольно быстро. Это более оптимизированное решение, чем пробегать
            //по каждому элементу коллекции
            String FullPostText = null;
            string json = null;
            //Саму частность храним в словаре. 
            Dictionary<char, double> massSymbPairs = new Dictionary<char, double>();
            //Итерационно пробегаем по листу, и присваиваем переменной FullPostText текст из постов
            list.ForEach(item => FullPostText += item.Text.ToUpper());

            foreach (var symbol in FullPostText.ToUpper())
            {
                //проверяем, что символ в строке - это буква
                if (Char.IsLetter(symbol))
                {
                    //проверяем, что в словаре такой буквы еще нет
                    if (!massSymbPairs.ContainsKey(symbol))
                    {
                        //Рассчитываем частность, и добавляем в словарь
                        double countOfSymbol = FullPostText.Count(item => (char)item == symbol);
                        double symbolFrequency = Math.Round(countOfSymbol / FullPostText.Length, 3);
                        massSymbPairs.Add(symbol, symbolFrequency);
                    }
                }
            }
            //Сериализуем словарь в JSON
            json = JsonConvert.SerializeObject(massSymbPairs);
            return json;
        }
    }
}
