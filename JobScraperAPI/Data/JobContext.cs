    using Microsoft.EntityFrameworkCore;

    public class JobContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; } // 定义 Jobs 数据表

        public JobContext(DbContextOptions<JobContext> options) : base(options)
        {
        }

        // 配置数据库连接
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=jobs.db"); // SQLite 数据库
            }
        }

    }


