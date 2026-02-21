using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrawlerApp.Services
{
    public static class DataParser
    {
        public static DateTimeOffset ParseToOffsetDate(this string input) 
        {
            var match = Regex.Match(input, @"\((?:МСК)([+-]\d+)\)");
            if (!match.Success)
                throw new FormatException("Не удалось извлечь смещение");

            var offset = int.Parse(match.Groups[1].Value);

            var datePart = Regex.Replace(input, @"\s*\(.*\)", "").Trim();

            DateTime date = DateTime.ParseExact(datePart, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
            return new DateTimeOffset(date, TimeSpan.FromHours(offset));
        }

        public static string? GetTextFromNode(this HtmlNode node)
        {
            if (node == null)
                return null;
            return node.InnerText;
        }
    }
}
