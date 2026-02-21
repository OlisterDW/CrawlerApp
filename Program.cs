using CrawlerApp.Services;
using HtmlAgilityPack;
using System;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string url = "https://zakupki.nefteavtomatika.ru";
            WebScrapService scrapService = new WebScrapService(url);

            var tables = scrapService.GetTables();

            var purchases = scrapService.GetPurchases(tables);

            string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            XmlWriter xmlWriter = new XmlWriter(path);
            xmlWriter.SavePurchases(purchases);

    }
    }
}