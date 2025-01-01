using JobScraper.API.Models;

namespace JobScraper.API.Interfaces {
public interface IScraperStrategy
{
    Task<List<Job>> ScrapeJobsAsync(string keyword, string location,  int page, int pageSize);

    Task<string> FetchJobDescription(string detailPageUrl);

    Task<List<Job>> ScrapeJobsByCategoryAsync(string categoryId);
}
}
