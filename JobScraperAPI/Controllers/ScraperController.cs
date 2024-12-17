using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class JobSearchController : ControllerBase
{
    private readonly JobSearchService _jobSearchService;
    private readonly JobContext _context;

    public JobSearchController(JobSearchService jobSearchService, JobContext context)
    {
        _jobSearchService = jobSearchService;
        _context = context;
    }

    [HttpGet]
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

    [HttpGet("skills")]
    public IActionResult FetchSkills()
    {
        // 获取所有职位描述
        var jobDescriptions = _context.Jobs
            .Select(j => j.Description) // 假设每个 Job 有一个 Description 字段
            .ToList();

        // 分析技能
        var skillCounts = ExtractAndCountSkills(jobDescriptions);

        // 返回统计结果
        return Ok(skillCounts);
    }

    private Dictionary<string, int> ExtractAndCountSkills(List<string> descriptions)
    {
        // 假设技能关键字集合
        var skillKeywords = new[] { "C#", "Java", "React", "SQL", "AWS", "Python", "JavaScript", "Docker" };

        // 初始化计数字典
        var skillCounts = skillKeywords.ToDictionary(skill => skill, skill => 0);

        foreach (var description in descriptions)
        {
            if (string.IsNullOrEmpty(description)) continue;

            foreach (var skill in skillKeywords)
            {
                if (description.IndexOf(skill, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    skillCounts[skill]++;
                }
            }
        }

        // 过滤掉未出现的技能
        return skillCounts.Where(kvp => kvp.Value > 0)
                          .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

}

