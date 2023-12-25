using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBS2.Service.Utils
{
    public static class TextUtils
    {
        public static string Capitalize(string text)
        {
            text = text.ToLower().Trim();
            string[] stringArray = text.Split(' ');
            for (int i = 0; i < stringArray.Length; i++)
            {
                char c = char.ToUpper(stringArray[i][0]);
                stringArray[i] = c.ToString() + stringArray[i].Remove(0, 1);
            }
            return string.Join(' ',stringArray);
        }
    }
}
