using Quartz;
using Microsoft.EntityFrameworkCore;

public class UpdateJobDescriptionTask : IJob
{
    private readonly JobContext _context;
    private readonly ScraperFactory _scraperFactory;

    public UpdateJobDescriptionTask(JobContext context, ScraperFactory scraperFactory)
    {
        _context = context;
        _scraperFactory = scraperFactory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"[{DateTime.Now}] - Starting UpdateJobDescriptionTask...");

        var scraper = _scraperFactory.GetScraper("seek");

        var jobsToUpdate = await _context.Jobs
            .Where(j => string.IsNullOrEmpty(j.Description) || j.Description == "N/A")
            .ToListAsync();

        foreach (var job in jobsToUpdate)
        {
            try
            {
                job.Description = await scraper.FetchJobDescription(job.Url);
                Console.WriteLine($"Updated Description for Job: {job.Title}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update description for Job: {job.Title}, Error: {ex.Message}");
            }
        }

        await _context.SaveChangesAsync();
        Console.WriteLine("Finished UpdateJobDescriptionTask.");
    }
}
