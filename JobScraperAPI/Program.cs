using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // 前端地址
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<JobContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 注册 IScraper 的实现
builder.Services.AddScoped<IScraperStrategy, SeekScraper>();
// builder.Services.AddScoped<IScraperStrategy, LinkedInScraper>();

// 注册 ScraperFactory
builder.Services.AddScoped<ScraperFactory>();

builder.Services.AddQuartz(q =>
{
    // 配置 Job 1: 更新 Description
    var updateJobKey = new JobKey("UpdateJobDescriptionTask");
    q.AddJob<UpdateJobDescriptionTask>(opts => opts.WithIdentity(updateJobKey));
    q.AddTrigger(opts => opts
        .ForJob(updateJobKey)
        .WithIdentity("UpdateJobDescriptionTask-trigger")
        .WithCronSchedule("0 0 * * * ?")); // 每小时执行一次
});


// 注册业务逻辑服务
builder.Services.AddScoped<JobSearchService>();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("AllowSpecificOrigins");
}

app.UseHttpsRedirection();


app.MapControllers();
app.Run();

