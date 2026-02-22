
using CrawlerApp.Models;
using System.Xml.Serialization;

namespace CrawlerApp.Services
{
    public class XmlWriter
    {
        private readonly string _path;

        public XmlWriter(string path)
        {
            _path = path;
        }

        public void SavePurchases(List<Purchase> purchases) 
        {
            XmlSerializer xmlSerializer = new (typeof(List<Purchase>), new XmlRootAttribute("Purchases"));
            using (FileStream fs = new($"{_path}/purchases.xml", FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, purchases);

                Console.WriteLine("Список закупок сохранен");
            }
        }
    }
}
