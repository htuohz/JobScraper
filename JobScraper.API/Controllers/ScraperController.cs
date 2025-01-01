using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobScraper.API.Data;

[ApiController]
[Route("api/[controller]")]
public class JobSearchController : ControllerBase
{
    private readonly JobScraperService _jobSearchService;
    private readonly JobContext _context;

    public JobSearchController(JobScraperService jobSearchService, JobContext context)
    {
        _jobSearchService = jobSearchService;
        _context = context;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchJobs(
        [FromQuery] string site = "seek",
        [FromQuery] string keyword = "developer",
        [FromQuery] string location = "sydney",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool forceRefresh = false)
    {
        if (string.IsNullOrEmpty(site) || string.IsNullOrEmpty(keyword) || string.IsNullOrEmpty(location))
        {
            return BadRequest("Site, keyword, and location are required.");
        }

        try
        {
            var result = await _jobSearchService.SearchJobsAsync(site, keyword, location, page, pageSize, forceRefresh);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }

    [HttpGet("top-locations")]
    public async Task<IActionResult> GetTopLocationsByJobTitle(string jobTitle, string limit = "10")
    {
        if (string.IsNullOrEmpty(jobTitle))
        {
            return BadRequest("Job title is required.");
        }

        var topLocations = await _context.Jobs
            .Where(j => j.Title.ToLower().Contains(jobTitle.ToLower()))
            .GroupBy(j => j.Location)
            .Select(g => new { Location = g.Key, Count = g.Count() })
            .OrderByDescending(l => l.Count)
            .Take(int.Parse(limit))
            .ToListAsync();

        return Ok(topLocations);
    }

    [HttpGet("top-locations-by-skills")]
    public async Task<IActionResult> GetTopLocationsBySkills(string[] skills, string limit = "10")
    {
        if (string.IsNullOrEmpty(skill))
        {
            return BadRequest("Skill is required.");
        }

        var topLocations = await _context.JobSkills
            .Where(js => skills.Contains(js.Skill))
            .GroupBy(js => js.Job.Location)
            .Select(g => new { Location = g.Key, Count = g.Count() })
            .OrderByDescending(l => l.Count)
            .Take(int.Parse(limit))
            .ToListAsync();

        return Ok(topLocations);
    }
}

