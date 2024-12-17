public class ScraperFactory
{
    private readonly Dictionary<string, IScraperStrategy> _scrapers;

    public ScraperFactory(IEnumerable<IScraperStrategy> scrapers)
    {
        _scrapers = scrapers.ToDictionary(scraper => scraper.GetType().Name.Replace("Scraper", "").ToLower());
    }

    public IScraperStrategy GetScraper(string siteName)
    {
        if (_scrapers.TryGetValue(siteName.ToLower(), out var scraper))
        {
            return scraper;
        }

        throw new KeyNotFoundException($"No scraper found for site: {siteName}");
    }
}
