using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FvpWebApp.Infrastructure
{
    public static class Utils
    {
        public static string StringToDigits(string str)
        {
            var cleanStr = Regex.Replace(str, @"[\s -]", "");
            StringBuilder stringBuilder = new StringBuilder();
            if (cleanStr.Length > 0)
            {
                foreach (var ch in cleanStr)
                {
                    stringBuilder.Append((int)ch);
                }
            }
            return stringBuilder.ToString().Length <= 100 ? stringBuilder.ToString() : stringBuilder.ToString().Substring(0, 100);
        }
    }
}
