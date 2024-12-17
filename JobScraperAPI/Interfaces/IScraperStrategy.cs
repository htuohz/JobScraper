public interface IScraperStrategy
{
    Task<List<Job>> ScrapeJobsAsync(string keyword, string location,  int page, int pageSize);

    Task<string> FetchJobDescription(string detailPageUrl);
}
