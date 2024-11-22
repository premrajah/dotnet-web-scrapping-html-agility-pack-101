// See https://aka.ms/new-console-template for more information
using dotnet_webscrapping.Models;
using HtmlAgilityPack;
using CsvHelper;
using System.Globalization;
using dotnet_webscrapping;

Console.WriteLine("Scrapping Project!");

string targetDemoUrl = "https://www.scrapingcourse.com/ecommerce/";
string targetAdvanceDemoUrl = "https://www.scrapingcourse.com/ecommerce/page/1/";

try
{
    Console.WriteLine("Starting...");

    // ScrappingCourseScrapper.ScrapDemoSite(targetDemoUrl);
    ScrappingCourseScrapper.AdvanceScrapeDemoSite(targetAdvanceDemoUrl);

    Console.WriteLine("Completed.");
}
catch (System.Exception ex)
{
    Console.WriteLine($"Error {ex.Message}");
}

