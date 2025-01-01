using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobScraper.API.Controllers;
using JobScraper.API.Models;
using JobScraper.API.Data;
using JobScraper.API.DTOs;

namespace JobScraper.Tests.Controllers
{

    public class SkillsControllerTest
    {
        private JobContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<JobContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            return new JobContext(options);
        }

        [Fact]
        public async Task GetSkillsByJobTitle_ReturnsSkills_WhenValidInput()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.JobSkills.AddRange(
                new JobSkill
                {
                    Skill = "C#",
                    Job = new Job { Id=1, Title = "Software Engineer", Location = "Melbourne", Source = "seek", Company = "Company A", Url = "https://www.seek.com.au/job/123", Description = "Job description", PostedDate = "2021-01-01" }
                },
                new JobSkill
                {
                    Skill = "JavaScript",
                    Job = new Job { Id=2, Title = "Frontend Developer", Location = "Melbourne", Source = "seek", Company = "Company B", Url = "https://www.seek.com.au/job/456", Description = "Job description", PostedDate = "2021-01-01" }
                }
            );
            await context.SaveChangesAsync();

            var controller = new SkillsController(context);

            // Act
            var result = await controller.GetSkillsByJobTitle("Software Engineer", "Melbourne", "10");
            

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var jobSkills = Assert.IsType<List<SkillCount>>(okResult.Value);
            Assert.NotEmpty(jobSkills);
            Assert.Equal(1, jobSkills.Count);
            Assert.Equal("C#", jobSkills[0].Skill);
        }

        [Fact]
        public async Task GetSkillsByJobTitle_ReturnsBadRequest_WhenJobTitleIsMissing()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new SkillsController(context);

            // Act
            var result = await controller.GetSkillsByJobTitle("", "Melbourne", "10");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Job title is required.", badRequestResult.Value);
        }
    }

}
