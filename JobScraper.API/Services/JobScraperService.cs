using JobScraper.API.Models;
using JobScraper.API.Interfaces;
using JobScraper.API.Factories;
using JobScraper.API.Data;

public class JobScraperService
{
    private readonly ScraperFactory _scraperFactory;
    private readonly JobContext _context;
    private readonly Dictionary<string, List<Job>> _cache;

    private readonly CategoryService _categoryService;

    public JobScraperService(ScraperFactory scraperFactory, JobContext context, CategoryService categoryService)
    {
        _scraperFactory = scraperFactory;
        _context = context;
        _cache = new Dictionary<string, List<Job>>();
        _categoryService = categoryService;
    }

    public async Task<PaginatedResult<Job>> SearchJobsAsync(string site, string keyword, string location, int page, int pageSize, bool forceRefresh)
    {
        // 1. 规范化输入
        var cacheKey = GenerateCacheKey(site, keyword, location);

        // 2. 缓存查询
        if (!forceRefresh && _cache.ContainsKey(cacheKey))
        {
            return await Paginate(_cache[cacheKey], site, page, pageSize);
        }

        // 3. 数据库查询
        var jobs = _context.Jobs
            .Where(j =>
                j.Title.ToLower().Contains(keyword.ToLower()) &&
                j.Location.ToLower().Contains(location.ToLower()) &&
                j.Source.ToLower() == site.ToLower())
            .ToList();

        if (jobs.Count > 0 && !forceRefresh)
        {
            _cache[cacheKey] = jobs;
            return await Paginate(jobs, site, page, pageSize);
        }

        // 4. 动态选择爬虫并实时爬取
        var scraper = _scraperFactory.GetScraper(site);
        var scrapedJobs = await scraper.ScrapeJobsAsync(keyword, location, page, pageSize);

        // 保存到数据库
        foreach (var job in scrapedJobs)
        {
            job.Source = site;
        }
        _context.Jobs.AddRange(scrapedJobs);
        await _context.SaveChangesAsync();

        // 更新缓存
        _cache[cacheKey] = scrapedJobs;

        return await Paginate(scrapedJobs, site, page, pageSize);
    }

    private async Task<PaginatedResult<Job>> Paginate(List<Job> jobs, string site, int page, int pageSize)
    {
        // 防止非法分页参数
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10; // 默认每页大小

        // 总记录数
        var totalCount = jobs.Count;

        // 计算总页数
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        // 如果 page 超过总页数，返回空数据
        if (page > totalPages)
        {
            return new PaginatedResult<Job>
            {
                Jobs = new List<Job>(),
                TotalPages = totalPages,
                TotalCount = totalCount,
                CurrentPage = page
            };
        }

        // 分页数据
        var paginatedJobs = jobs.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        // 动态抓取 Description
        var scraper = _scraperFactory.GetScraper(site);
        foreach (var job in paginatedJobs)
        {
            if (string.IsNullOrEmpty(job.Description))
            {
                try
                {
                    var descriptionTasks = paginatedJobs
                        .Where(j => string.IsNullOrEmpty(j.Description) || j.Description.Equals("N/A"))
                        .Select(async job =>
                        {
                            job.Description = await scraper.FetchJobDescription(job.Url) ?? "N/A";
                            return job;
                        });

                    await Task.WhenAll(descriptionTasks);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching description for job {job.Title}: {ex.Message}");
                }
            }
        }

        return new PaginatedResult<Job>
        {
            Jobs = paginatedJobs,
            TotalPages = totalPages,
            TotalCount = totalCount,
            CurrentPage = page
        };
    }

    private string GenerateCacheKey(string site, string keyword, string location)
    {
        keyword = keyword.Trim().ToLower();
        location = location.Trim().ToLower();
        return $"{site}_{keyword}_{location}";
    }

    public async Task ScrapeJobsByCategoriesAsync(string site)
    {
        // Load SEEK categories
        var categories = _categoryService.GetCategoriesBySite(site);
        var scraper = _scraperFactory.GetScraper(site);

        foreach (var category in categories)
        {
            Console.WriteLine($"Scraping jobs for category: {category.Name} ({category.Id})");

            // Scrape jobs for the current category
            var jobs = await scraper.ScrapeJobsByCategoryAsync(category.Id);

            // Save jobs to the database
            foreach (var job in jobs)
            {
                if (!_context.Jobs.Any(j => j.Title == job.Title && j.Company == job.Company))
                {
                    _context.Jobs.Add(job);
                }
            }

            await _context.SaveChangesAsync();
        }

        Console.WriteLine("Finished scraping jobs for all categories.");
    }
}

public class PaginatedResult<T>
{
    public List<T> Jobs { get; set; } // 当前页的数据
    public int TotalPages { get; set; } // 总页数
    public int TotalCount { get; set; } // 总记录数
    public int CurrentPage { get; set; } // 当前页码
}
