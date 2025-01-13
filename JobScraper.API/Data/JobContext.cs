using Microsoft.EntityFrameworkCore;
using JobScraper.API.Models;

namespace JobScraper.API.Data
{
    public class JobContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; } // 定义 Jobs 数据表
        public DbSet<JobSkill> JobSkills { get; set; } // 定义 JobSkills 数据表

        public JobContext(DbContextOptions<JobContext> options) : base(options)
        {
        }

    }
}



