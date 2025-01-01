using JobScraper.API.Interfaces;
using JobScraper.API.Services;

public class CategoryScraperFactory
{
    private readonly IServiceProvider _serviceProvider;

    public CategoryScraperFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ICategoryScraper GetScraper(string site)
    {
        return site.ToLower() switch
        {
            "seek" => _serviceProvider.GetService<SeekCategoryScraper>(),
            // "linkedin" => _serviceProvider.GetService<LinkedInCategoryScraper>(),
            _ => throw new ArgumentException($"Unsupported site: {site}")
        };
    }
}
