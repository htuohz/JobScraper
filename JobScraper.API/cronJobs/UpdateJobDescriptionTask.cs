using Quartz;
using Microsoft.EntityFrameworkCore;
using JobScraper.API.Models;
using JobScraper.API.Interfaces;
using JobScraper.API.Factories;
using JobScraper.API.Data;


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
    int batchSize = 100;
    int updatedCount = 0;

        // 查询待更新的记录（分批加载）
    var jobsToUpdate = await _context.Jobs
        .Where(j => string.IsNullOrEmpty(j.Description) || j.Description == "N/A" || j.Description == "")
        .OrderByDescending(j => j.CreatedAt)
        .Take(batchSize)
        .ToListAsync();

    foreach (var job in jobsToUpdate)
    {
        try
        {
            // 获取并更新 Job 描述
            job.Description = await scraper.FetchJobDescription(job.Url);
            Console.WriteLine($"Updated Description for Job: {job.Title}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update description for Job: {job.Title}, Error: {ex.Message}");
        }
    }

    // 批量保存更新后的记录
    await _context.SaveChangesAsync();
    updatedCount += jobsToUpdate.Count;

    Console.WriteLine($"Batch updated {jobsToUpdate.Count} jobs. Total updated so far: {updatedCount}.");

    Console.WriteLine($"Finished UpdateJobDescriptionTask. Total jobs updated: {updatedCount}.");
}

}
