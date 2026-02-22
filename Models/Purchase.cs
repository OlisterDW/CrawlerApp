namespace CrawlerApp.Models
{
    public class Purchase
    {
        public Purchase(){ }
        public Purchase(int number, string customer, Details details)
        {
            Number = number;
            Customer = customer;
            Description = details.Description;
            ApplicationStart = details.ApplicationStart;
            ApplicationEnd = details.ApplicationEnd;
            TotalPrice = details.TotalPrice;
            ContactPersonName = details.ContactPersonName;
            ContactPersonPhone = details.ContactPersonPhone;
            Items = details.Items;
        }

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
