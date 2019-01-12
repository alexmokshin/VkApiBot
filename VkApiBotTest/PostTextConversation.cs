using System;
using System.Collections.Generic;
using System.Linq;
using VkNet.Model.Attachments;
using Newtonsoft.Json;

namespace VkApiBotTest
{
    class PostTextConversation
    {
        public static string JsonPostConversation(List<Post> list)
        {
            String FullPostText = null;
            string json = null;
            Dictionary<char, double> massSymbPairs = new Dictionary<char, double>();
            list.ForEach(item => FullPostText += item.Text.ToUpper());
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
            json = JsonConvert.SerializeObject(massSymbPairs);
            return json;
        }
    }
}
