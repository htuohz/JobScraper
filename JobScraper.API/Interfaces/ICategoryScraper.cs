
namespace JobScraper.API.Interfaces {
public interface ICategoryScraper
{
    Task<List<Category>> FetchCategoriesAsync();
}
}
