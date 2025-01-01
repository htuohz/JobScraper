using Newtonsoft.Json;

public class CategoryService
{
    private readonly string _filePath;

    public CategoryService(string filePath)
    {
        _filePath = filePath;
    }

    // 读取 JSON 文件中的分类数据
    public Dictionary<string, List<Category>> LoadCategories()
    {
        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException($"Category file not found at: {_filePath}");
        }

        var json = File.ReadAllText(_filePath);
        return JsonConvert.DeserializeObject<Dictionary<string, List<Category>>>(json);
    }

    // 保存分类数据到 JSON 文件
    public void SaveCategories(Dictionary<string, List<Category>> categories)
    {
        var json = JsonConvert.SerializeObject(categories, Formatting.Indented);
        File.WriteAllText(_filePath, json);
    }

    // 根据网站名称获取分类列表
    public List<Category> GetCategoriesBySite(string site)
    {
        var categories = LoadCategories();
        return categories.ContainsKey(site) ? categories[site] : new List<Category>();
    }

        public void AddOrUpdateCategories(string site, List<Category> newCategories)
    {
        var categories = LoadCategories();

        if (!categories.ContainsKey(site))
        {
            categories[site] = new List<Category>();
        }

        foreach (var newCategory in newCategories)
        {
            // Check if the category already exists
            if (!categories[site].Any(c => c.Id == newCategory.Id))
            {
                categories[site].Add(newCategory);
            }
        }

        SaveCategories(categories);
    }
}

// 分类模型
public class Category
{
    public string Id { get; set; } // 分类 ID
    public string Name { get; set; } // 分类名称
}
