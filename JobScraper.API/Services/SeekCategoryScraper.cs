using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JobScraper.API.Interfaces;


namespace JobScraper.API.Services
{

public class SeekCategoryScraper : ICategoryScraper
{
    private readonly HttpClient _client;

    public SeekCategoryScraper(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<Category>> FetchCategoriesAsync()
    {
        var url = "https://www.seek.com.au/jobs";
        var html = await _client.GetStringAsync(url);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var categories = new List<Category>();

        // Target the #classificationsPanel > nav structure
        var classificationNav = doc.DocumentNode.SelectSingleNode("//div[@id='classificationsPanel']/nav");
        if (classificationNav == null)
        {
            Console.WriteLine("No classifications panel found in the HTML.");
            return categories;
        }

        // Find all category links within the navigation
        var categoryNodes = classificationNav.SelectNodes(".//a[@data-automation]");
        if (categoryNodes != null)
        {
            foreach (var node in categoryNodes)
            {
                try
                {
                    // Extract ID from `data-automation` or `data-testid`
                    var id = node.GetAttributeValue("data-automation", string.Empty);

                    // Extract category name from nested <span>
                    var nameNode = node.SelectSingleNode(".//span[@data-automation='item-text']");
                    var name = nameNode?.InnerText.Trim();

                    if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name))
                    {
                        categories.Add(new Category
                        {
                            Id = id,
                            Name = name
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing category node: {ex.Message}");
                }
            }
        }
        else
        {
            Console.WriteLine("No category nodes found in the classifications panel.");
        }

        return categories;
    }
}
}
