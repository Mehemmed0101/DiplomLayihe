using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiplomLayihe.AppCodee.Extensions
{
    public static partial class Extension
    {
        public static string HtmlToPlainText(this string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return html;

            return Regex.Replace(html, "<(.|\n)*?>", "");
            
        }
        public static string ToEllipse(this string text, int length = 20)
        {
            if (string.IsNullOrWhiteSpace(text) || length >= text.Length)
                return text;

            return $"{text.Substring(0, length)}...";
        }
    }
}
