namespace CrawlerApp.Models
{
    public class Purchase
    {
        public string Url { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public DateTimeOffset ApplicationStart { get; set; }
        public DateTimeOffset ApplicationEnd { get; set; }
        public decimal? TotalPrice { get; set;  }
        public string ContactPersonName { get; set; }
        public string ContactPersonPhone { get; set; }
        public string Customer {  get; set; }
        public List<Item> Items { get; set; } = new List<Item>();

    }
}
