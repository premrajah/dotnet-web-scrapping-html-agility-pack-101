using dotnet_webscrapping.Models;
using HtmlAgilityPack;
using CsvHelper;
using System.Globalization;
using Microsoft.VisualBasic;


namespace dotnet_webscrapping
{
    public static class ScrappingCourseScrapper
    {
        private static HtmlWeb web = new();

        public static void ScrapDemoSite(string targetUrl)
        {
            var document = web.Load(targetUrl);
            var products = new List<Product>();

            var productHtmlElements = document.DocumentNode.QuerySelectorAll("li.product");

            foreach (var productHtmlElement in productHtmlElements)
            {
                var name = HtmlEntity.DeEntitize(productHtmlElement.QuerySelector("h2").InnerText);
                var url = HtmlEntity.DeEntitize(productHtmlElement.QuerySelector("a").Attributes["href"].Value);
                var image = HtmlEntity.DeEntitize(productHtmlElement.QuerySelector("img").Attributes["src"].Value);
                var price = HtmlEntity.DeEntitize(productHtmlElement.QuerySelector(".price").InnerText);
                var product = new Product() { Name = name, Url = url, Image = image, Price = price };
                products.Add(product);
            }


            CreateCSV(products);

        }

        public static void CreateCSV(List<Product> products, string documentName = "products.csv")
        {
            // download csv 
            using var writer = new StreamWriter(documentName);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(products);
        }


        public static void AdvanceScrapeDemoSite(string targetUrl)
        {
            var products = new List<Product>();
            var firstPageToScrap = targetUrl;
            var pagesDiscovered = new List<string> { firstPageToScrap };
            var pagesToScrap = new Queue<string>();
            pagesToScrap.Enqueue(firstPageToScrap);

            // current crawling iteration
            int i = 0;
            // maximum number of pages to scrap before stopping 
            int limit = 10;

            // scrap until limit has been reached
            while (pagesToScrap.Count != 0 && i < limit)
            {
                var currentPage = pagesToScrap.Dequeue();
                // loading the page
                var currentDocument = web.Load(currentPage);

                // selecting the list of pagination HTML elements 
                var paginationHTMLElements = currentDocument.DocumentNode.QuerySelectorAll("a-page-numbers");

                // avoid visiting page twice 
                foreach (var paginationHTMLElement in paginationHTMLElements)
                {
                    // extract the current pagination URL
                    var newPaginationLink = paginationHTMLElement.Attributes["href"].Value;

                    // if discovered is new 
                    if (!pagesDiscovered.Contains(newPaginationLink))
                    {
                        // if page discovered needs to be scrapped
                        if (!pagesToScrap.Contains(newPaginationLink))
                        {
                            pagesToScrap.Enqueue(newPaginationLink);
                        }
                        pagesDiscovered.Add(newPaginationLink);
                    }
                }


                // scrapping logic
                var productHTMLElements = currentDocument.DocumentNode.QuerySelectorAll("li.product");
                // iterating over the list of product HTML elements 
                foreach (var productHTMLElement in productHTMLElements)
                {
                    // scraping logic 
                    var url = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("a").Attributes["href"].Value);
                    var image = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("img").Attributes["src"].Value);
                    var name = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("h2").InnerText);
                    var price = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector(".price").InnerText);
                    var product = new Product() { Url = url, Image = image, Name = name, Price = price };
                    products.Add(product);
                }

                i++;
            }

            CreateCSV(products, "advance_products.csv");
        }
    }

}

