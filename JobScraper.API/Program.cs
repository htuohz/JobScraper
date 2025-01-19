using Microsoft.EntityFrameworkCore;
using Quartz;
using JobScraper.API.Interfaces;
using JobScraper.API.Services;
using JobScraper.API.Factories;
using JobScraper.API.Data;



var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:5173") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
    options.AddPolicy("AllowProdOrigins", policy =>
    {
        policy.WithOrigins("https://job-scraper-g5jd9v22t-tianhao-zhous-projects-b0a71b1b.vercel.app") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    }); 
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    var certificatePath = Environment.GetEnvironmentVariable("CERTIFICATE_PATH") ?? "/home/ec2-user/localhost.pfx";
    var certificatePassword = Environment.GetEnvironmentVariable("CERTIFICATE_PASSWORD") ?? "000000";

    serverOptions.ListenAnyIP(8080); // HTTP
    serverOptions.ListenAnyIP(8443, listenOptions =>
    {
        listenOptions.UseHttps(certificatePath, certificatePassword);
    });
    Console.WriteLine($"Using Certificate Path: {certificatePath}");
    Console.WriteLine($"Using Certificate Password: {certificatePassword}");
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<JobContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 注册 IScraper 的实现
builder.Services.AddScoped<IScraperStrategy, SeekScraper>();
// builder.Services.AddScoped<IScraperStrategy, LinkedInScraper>();

// 注册 ScraperFactory
builder.Services.AddScoped<ScraperFactory>();

builder.Services.AddScoped<UpdateJobDescriptionTask>();

builder.Services.AddScoped<AnalyzeJobSkillsTask>();

builder.Services.AddScoped<SeekCategoryScraper>();
// builder.Services.AddScoped<LinkedInCategoryScraper>();
builder.Services.AddSingleton<CategoryScraperFactory>();
var filePath = builder.Configuration.GetValue<string>("CategoryFilePath");
builder.Services.AddSingleton(new CategoryService(filePath));

// Register SeekCategoryScraper
builder.Services.AddScoped<SeekCategoryScraper>();

// Register CategoryUpdater
builder.Services.AddScoped<CategoryUpdater>();

builder.Services.AddScoped<ICategoryScraper, SeekCategoryScraper>();


builder.Services.AddQuartz(q =>
{
    // 配置 Job 1: 更新 Description
    var updateJobKey = new JobKey("UpdateJobDescriptionTask");
    q.AddJob<UpdateJobDescriptionTask>(opts => opts.WithIdentity(updateJobKey));
    q.AddTrigger(opts => opts
        .ForJob(updateJobKey)
        .WithIdentity("UpdateJobDescriptionTask-trigger")
        .WithCronSchedule("0 0 * * * ?")); // 每小时执行一次
    
    // 提取技能任务的 Key
    var analyzeSkillsJobKey = new JobKey("AnalyzeJobSkillsTask");

    // 注册 Job
    q.AddJob<AnalyzeJobSkillsTask>(opts => opts.WithIdentity(analyzeSkillsJobKey));

    // 注册 Trigger
    q.AddTrigger(opts => opts
        .ForJob(analyzeSkillsJobKey) // 关联 Job
        .WithIdentity("AnalyzeJobSkillsTask-trigger")
        .WithCronSchedule("0 0 * * * ?")); // 每10分钟执行一次


    q.AddJob<UpdateCategoriesJob>(opts => opts.WithIdentity("UpdateCategoriesJob"));
    q.AddTrigger(opts => opts
        .ForJob("UpdateCategoriesJob")
        .WithIdentity("UpdateCategoriesTrigger")
        .WithCronSchedule("0 0 0 * * ?")); // Daily at midnight 
    
    q.AddJob<ScrapeJobsJob>(opts => opts.WithIdentity("ScrapeJobsJob"));
    q.AddTrigger(opts => opts
        .ForJob("ScrapeJobsJob")
        .WithIdentity("ScrapeJobsTrigger")
        .WithCronSchedule("0 0 0 * * ?")); 
});
builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true; // 等待所有任务完成后再停止
    options.StartDelay = TimeSpan.Zero;   // 确保服务立即启动
    Console.WriteLine("Quartz Hosted Service Registered!");
});


// 注册业务逻辑服务
builder.Services.AddScoped<JobScraperService>();
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<JobContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowSpecificOrigins");
}
if(app.Environment.IsProduction())
{
    app.UseCors("AllowProdOrigins");
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.MapGet("/", () => "API is running!");
app.Run();

