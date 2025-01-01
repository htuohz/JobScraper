using Quartz;
using System;
using System.Threading.Tasks;
using JobScraper.API.Services;
using JobScraper.API.Data;


public class UpdateCategoriesJob : IJob
{
    private readonly CategoryUpdater _categoryUpdater;

    public UpdateCategoriesJob(CategoryUpdater categoryUpdater)
    {
        _categoryUpdater = categoryUpdater;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"[{DateTime.Now}] Starting category update job...");
        await _categoryUpdater.UpdateCategoriesAsync("SEEK");
        Console.WriteLine($"[{DateTime.Now}] Finished category update job.");
    }
}

