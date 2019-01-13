using System;
using System.Collections.Generic;
using System.Linq;
using VkNet.Model.Attachments;
using Newtonsoft.Json;

namespace VkApiBotTest
{
    class UserPostActivityConvertor
    {
        //Данный метод будет возвращать строку JSON из листа с постами
        public static string ConvertListToJson(List<Post> list)
        {
            //В ходе анализа, чтобы анализировать частотность букв, уместее все приводить к одной строке, и уже на ее основании проводить расчет
            //Идея в том, что пока размер строки не превышает 4 Гб, расчет будет идти довольно быстро. Это более оптимизированное решение, чем пробегать
            //по каждому элементу коллекции
            String FullPostText = null;
            //string json = null;
            double countOfSymbol = 0;
            double symbolFrequency = 0;
            //Саму частность храним в словаре. 
            Dictionary<char, double> massSymbPairs = new Dictionary<char, double>();
            //Итерационно пробегаем по листу, и присваиваем переменной FullPostText текст из постов
            list.ForEach(item => FullPostText += item.Text.ToUpper());

            foreach (var symbol in FullPostText.ToUpper())
            {
                //проверяем, что символ в строке - это буква и проверяем, что в словаре такой буквы еще нет
                if (Char.IsLetter(symbol) && !massSymbPairs.ContainsKey(symbol))
                {
                    //Рассчитываем частность, и добавляем в словарь
                    countOfSymbol = FullPostText.Count(item => item == symbol);
                    symbolFrequency = Math.Round(countOfSymbol / FullPostText.Length, 3);
                    massSymbPairs.Add(symbol, symbolFrequency);
                }
            }

            return JsonConvert.SerializeObject(massSymbPairs);
        }
    }
}
