namespace CrawlerApp.Models
{
    public class Item
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Measurement { get; set; }

        public Price CurrentPrice { get; set; }

    }
}
