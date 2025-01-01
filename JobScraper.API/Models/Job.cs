using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobScraper.API.Models
{
public class Job
{
    [Key] // Marks this as the primary key
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment for primary key
    public int Id { get; set; }

    [Required] // Ensures this property is non-nullable
    [MaxLength(255)] // Sets a maximum length for the column
    public string Title { get; set; } // Job title

    [MaxLength(255)] // Optional with a max length constraint
    public string Company { get; set; } // Company name

    [MaxLength(255)] // Optional with a max length constraint
    public string Location { get; set; } // Job location

    [MaxLength(50)] // Optional for a short text format like "2 days ago"
    public string PostedDate { get; set; } // Posting date

    [MaxLength(500)] // Allow a longer URL if necessary
    public string Url { get; set; } // Job URL

    [MaxLength(50)] // Store source like "SEEK" or "LinkedIn"
    public string Source { get; set; } // Data source

    [Required] // Ensures this property is non-nullable
    public DateTime CreatedAt { get; set; } // Creation timestamp

    [MaxLength(5000)] // Allow a longer description if needed
    public string Description { get; set; } // Full job description

    public Job()
    {
        CreatedAt = DateTime.UtcNow; // 使用 UTC 时间
    }
}
}
