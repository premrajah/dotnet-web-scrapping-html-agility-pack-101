// See https://aka.ms/new-console-template for more information
using dotnet_webscrapping.Models;
using HtmlAgilityPack;

Console.WriteLine("Scrapping Project!");

string targetUrl = "https://www.scrapingcourse.com/ecommerce/";
var web = new HtmlWeb();
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
