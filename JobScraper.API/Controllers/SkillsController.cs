using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobScraper.API.Data;
using JobScraper.API.DTOs;
namespace JobScraper.API.Controllers
{
[ApiController]
[Route("api/[controller]")]


public class SkillsController : ControllerBase
{
    private readonly JobContext _context;

    public SkillsController(JobContext context)
    {
        _context = context;
    }


    // 查询最常见的技能及统计
    [HttpGet("top-skills")]
    public async Task<IActionResult> GetTopSkills()
    {
        var topSkills = await _context.JobSkills
            .Where(js => js.Skill != "No skills found")
            .GroupBy(js => js.Skill)
            .Select(g => new { Skill = g.Key, Count = g.Count() })
            .OrderByDescending(s => s.Count)
            .Take(20)
            .ToListAsync();

        return Ok(topSkills);
    }

    [HttpGet("getSkillsByJobTitle")]
    public async Task<IActionResult> GetSkillsByJobTitle(string jobTitle, string location, string limit)
    {
        if (string.IsNullOrEmpty(jobTitle))
        {
            return BadRequest("Job title is required.");
        }

        var jobSkills = await _context.JobSkills
            .Where(js => js.Job.Title.ToLower().Contains(jobTitle.ToLower()) &&
                         js.Job.Location.ToLower().Contains(location.ToLower()) &&
                         js.Skill != "No skills found")
            .GroupBy(js => js.Skill)
            .Select(g => new SkillCount { Skill = g.Key, Count = g.Count() })
            .OrderByDescending(s => s.Count)
            .Take(int.Parse(limit))
            .ToListAsync();

        return Ok(jobSkills);
    }

    [HttpGet("getTopLocationsBySkills")]
    public async Task<IActionResult> GetTopLocationsBySkills(string[] skills, string limit = "10")
    {
        if (string.IsNullOrEmpty(skills))
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
}
