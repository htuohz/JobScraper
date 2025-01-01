using Quartz;
using JobScraper.API.Data;


public class ScrapeJobsJob : IJob
{
    private readonly JobScraperService _jobScraperService;

    public ScrapeJobsJob(JobScraperService jobScraperService)
    {
        _jobScraperService = jobScraperService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"[{DateTime.Now}] Starting job scraping...");
        await _jobScraperService.ScrapeJobsByCategoriesAsync("SEEK");
        Console.WriteLine($"[{DateTime.Now}] Finished job scraping.");
    }
}
