using CrawlerApp.Models;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace CrawlerApp.Services
{
    public class WebScrapService
    {
        private readonly string _url;

        public WebScrapService(string url)
        {
            _url = url;
        }

        public List<Purchase> GetPurchases(List<HtmlNode> nodes)
        {
            List<Purchase> purchases = new List<Purchase>();

            foreach (HtmlNode node in nodes)
            {
                var purchase = GetPurchase(node);
                purchases.Add(purchase);
            }

            return purchases;
        }

        private Purchase GetPurchase(HtmlNode node)
        {
            var number = node
                    .SelectSingleNode("./div/div[1]/div/div[1]/div[1]/b").GetTextFromNode();
            _ = int.TryParse(number, out int parsedNumber);

            var customer = node
                .SelectSingleNode("./div/div[1]/div/div[1]/div[2]").GetTextFromNode();

            var address = _url + node
                .SelectSingleNode("./div/div[2]/a").GetAttributeValue("href", string.Empty);

            var details = GetDetails(address);
            
            var description = details
                .SelectSingleNode(".//tbody/tr[3]/td[2]").GetTextFromNode();

            var applicationStart = details
                .SelectSingleNode(".//tbody/tr[4]/td[2]").GetTextFromNode();

            var applicationEnd = details
                .SelectSingleNode(".//tbody/tr[5]/td[2]").GetTextFromNode();

            var priceLimit = details
                .SelectSingleNode(".//tbody/tr[10]/td[2]").GetTextFromNode();

            _ = decimal.TryParse(priceLimit, out decimal price);

            var contactName = details
                .SelectSingleNode(".//tbody/tr[2]/td[2]/div[1]").GetTextFromNode();

            var contactPhone = details
                .SelectSingleNode(".//tbody/tr[2]/td[2]/div[2]").GetTextFromNode();

            var tableName =details.SelectSingleNode("//div[contains(text(),'Перечень требуемых позиций')]");
            List<Item> items = new List<Item>();

            if (tableName != null)
            {
                var rows = details.SelectNodes("./div/div[4]/div/div/div/div/div[6]/div[2]/div/table/tbody/tr");
                items = GetItems(rows, price);
            }
            
            var purchase = new Purchase()
            {
                Url = address,
                Number = parsedNumber,
                Description = description,
                ApplicationStart = applicationStart.ParseToOffsetDate(),
                ApplicationEnd = applicationEnd.ParseToOffsetDate(),
                TotalPrice = price,
                ContactPersonName = contactName,
                ContactPersonPhone = contactPhone,
                Customer = customer,
               Items = items,
            };

            return purchase;
        }

        private  static List<Item> GetItems(HtmlNodeCollection rows, decimal price)
        {
            List<Item> purchaseItem = new List<Item>();
            
            foreach (var row in rows) 
            {
                var name = row.SelectSingleNode("./td[2]").GetTextFromNode();
                var quantity = row.SelectSingleNode("./td[6]").GetTextFromNode();
                var measurement = row.SelectSingleNode("./td[5]").GetTextFromNode();

                _ = int.TryParse(quantity, out int resultQuantity);

                Item item = new Item
                {
                    Name = name,
                    Quantity = resultQuantity,
                    Measurement = measurement,
                    CurrentPrice = new Price
                    {
                        Currency = Currency.RUB,
                        Amount = price
                    }
                };

                purchaseItem.Add(item);
            }

            return purchaseItem;
         }
        

        private static HtmlNode GetDetails(string address)
        {
            var options = new ChromeOptions();
            using (var driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(address);

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                wait.Until(d => d.FindElements(By.XPath("//tr")).Count > 0);
                string pageSource = driver.PageSource;
                var doc = new HtmlDocument();
                doc.LoadHtml(pageSource);

                var node = doc.DocumentNode
                    .SelectSingleNode("//main");

                return node;
            }
        }

        public List<HtmlNode> GetTables()
        {
            var result = new List<HtmlNode>();

            using var driver = new ChromeDriver();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            driver.Navigate().GoToUrl(_url + "/purchases?active");

            while (true)
            {
                wait.Until(d => d.FindElements(By.XPath("//tr")).Count > 0);

                var firstRowText = driver
                    .FindElement(By.XPath("(//tr)[2]"))
                    .Text;

                var doc = new HtmlDocument();
                doc.LoadHtml(driver.PageSource);

                var rows = doc.DocumentNode.SelectNodes("//tr");
                if (rows != null)
                    result.AddRange(rows);

                var nextButton = driver.FindElement(
                    By.XPath("//i[contains(@class,'fa-chevron-right')]/ancestor::button")
                );

                if (!nextButton.Enabled)
                    break;

                nextButton.Click();

                wait.Until(d =>
                {
                    var newText = d.FindElement(By.XPath("(//tr)[2]")).Text;    
                    return newText != firstRowText;
                });
            }

            return result;
        }
    }
 }
