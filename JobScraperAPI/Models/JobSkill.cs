using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class JobSkill
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int JobId { get; set; } // 关联 Job 表的外键
    public string Skill { get; set; }
    public DateTime AnalyzedAt { get; set; } // 分析时间

    public Job Job { get; set; } // 导航属性
}
