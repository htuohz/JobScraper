using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using JobScraper.API.Models;
using JobScraper.API.Interfaces;
using JobScraper.API.Data;


namespace JobScraper.API.Services
{
public class SeekScraper : IScraperStrategy
{
    private readonly HttpClient _httpClient;

    public SeekScraper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Job>> ScrapeJobsAsync(string keyword, string location, int page, int pageSize)
    {
        var jobs = new List<Job>();
        bool hasMorePages = true;

        while (hasMorePages)
        {
            var url = BuildUrl(keyword, location, page);
            var html = await FetchHtmlAsync(url);
            if (string.IsNullOrEmpty(html))
            {
                break; // 如果没有返回 HTML，终止爬取
            }

            var parsedJobs = ParseJobsFromHtml(html);

            if (parsedJobs.Any())
            {
                jobs.AddRange(parsedJobs);
                page++; // 下一页
            }
            else
            {
                hasMorePages = false; // 没有更多数据，终止分页
            }
        }




        return jobs;
    }

    private string BuildUrl(string keyword, string location, int page)
    {
        keyword = Uri.EscapeDataString(keyword);
        location = Uri.EscapeDataString(location);
        return $"https://www.seek.com.au/jobs?keywords={keyword}&location={location}&page={page}";
    }

    private async Task<string> FetchHtmlAsync(string url)
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode(); // 确保响应成功
            return await response.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error fetching URL {url}: {ex.Message}");
            return null; // 返回空字符串以表示失败
        }
    }

    private List<Job> ParseJobsFromHtml(string html)
    {
        var jobs = new List<Job>();
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var jobCards = doc.DocumentNode.SelectNodes("//article[contains(@data-automation, 'normalJob')]");

        if (jobCards == null)
        {
            Console.WriteLine("No job cards found in the HTML.");
            return jobs; // 如果没有找到职位卡片，返回空列表
        }

        foreach (var card in jobCards)
        {
            try
            {
                var titleNode = card.SelectSingleNode(".//a[contains(@data-automation, 'jobTitle')]");
                var companyNode = card.SelectSingleNode(".//a[contains(@data-automation, 'jobCompany')]");
                var locationNode = card.SelectSingleNode(".//a[contains(@data-automation, 'jobLocation')]");
                var dateNode = card.SelectSingleNode(".//span[contains(@class, 'date')]");


                if (titleNode == null || companyNode == null || locationNode == null)
                {
                    continue; // 如果任何关键字段缺失，跳过这条职位
                }

                jobs.Add(new Job
                {
                    Title = titleNode.InnerText.Trim(),
                    Company = companyNode.InnerText.Trim(),
                    Location = locationNode.InnerText.Trim(),
                    PostedDate = dateNode?.InnerText.Trim() ?? "N/A", // 如果日期缺失，设置默认值
                    Url = "https://www.seek.com.au" + titleNode.GetAttributeValue("href", string.Empty),
                    Source = "SEEK",
                    CreatedAt = DateTime.UtcNow,
                    Description = ""

                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing job card: {ex.Message}");
                // 跳过当前职位，继续解析其他职位
            }
        }


        return jobs;
    }

    public async Task<List<Job>> ScrapeJobsByCategoryAsync(string categoryId)
    {
        var jobs = new List<Job>();
        int currentPage = 1;
        bool hasMorePages = true;

        while (hasMorePages)
        {
            var url = $"https://www.seek.com.au/jobs?classification={categoryId}&page={currentPage}";
            Console.WriteLine($"Fetching URL: {url}");

            var html = await FetchHtmlAsync(url);
            if (string.IsNullOrEmpty(html))
            {
                break; // 如果没有返回 HTML，终止爬取
            }

            var parsedJobs = ParseJobsFromHtml(html);

            if (parsedJobs.Any())
            {
                jobs.AddRange(parsedJobs);
                currentPage++;
                await Task.Delay(new Random().Next(1000, 3000)); // Avoid being flagged as a bot
            }
            else
            {
                hasMorePages = false; // 没有更多数据，终止分页
            }

        }

        return jobs;
    }

    public async Task<string> FetchJobDescription(string detailPageUrl)
    {
        var client = new HttpClient();
        var html = await client.GetStringAsync(detailPageUrl);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // 选择 description 的节点
        var descriptionNode = doc.DocumentNode.SelectSingleNode("//div[contains(@data-automation, 'jobAdDetails')]");

        return descriptionNode?.InnerText.Trim() ?? "No description available";
    }

}
}
