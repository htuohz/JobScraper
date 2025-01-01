using System;
using System.Threading.Tasks;
using JobScraper.API.Services;
using JobScraper.API.Interfaces;

public class CategoryUpdater
{
    private readonly ICategoryScraper _categoryScraper;
    private readonly CategoryService _categoryService;

    public CategoryUpdater(ICategoryScraper categoryScraper, CategoryService categoryService)
    {
        _categoryScraper = categoryScraper;
        _categoryService = categoryService;
    }

    // Updates categories for a specific site
    public async Task UpdateCategoriesAsync(string site)
    {
        Console.WriteLine($"Fetching categories for site: {site}");

        // Fetch categories from the scraper
        var fetchedCategories = await _categoryScraper.FetchCategoriesAsync();

        // Add or update categories in the JSON file
        _categoryService.AddOrUpdateCategories(site, fetchedCategories);

        Console.WriteLine("Categories successfully updated and saved to JSON.");
    }
}
