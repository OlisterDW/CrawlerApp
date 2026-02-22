using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlerApp.Models
{
    public class Details
    {
        public string Description { get; set; }
        public DateTimeOffset ApplicationStart { get; set; }
        public DateTimeOffset ApplicationEnd { get; set; }
        public decimal? TotalPrice { get; set;  }
        public string ContactPersonName { get; set; }
        public string ContactPersonPhone { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
    }
}
