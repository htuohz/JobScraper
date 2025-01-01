using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Quartz;
using JobScraper.API.Models;
using JobScraper.API.Data;

public class AnalyzeJobSkillsTask : IJob
{
    private readonly JobContext _context;

    public AnalyzeJobSkillsTask(JobContext context)
    {
        _context = context;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"[{DateTime.Now}] - Starting AnalyzeJobSkillsTask...");

        // 技能关键词列表
        var skillKeywords = new HashSet<string>
    {
        // Technical Skills
        "Python", "Java", "C#", "JavaScript", "SQL", "TypeScript", "Go", "PHP", "Ruby",
        "React", "Angular", "Django", "Flask", "Spring Boot", ".NET Core",
        "Git", "Docker", "Kubernetes", "Jenkins", "Ansible", "Terraform",
        "AWS", "Azure", "Google Cloud",
        "MySQL", "PostgreSQL", "MongoDB", "Redis", "Oracle",
        "TensorFlow", "PyTorch", "Scikit-Learn", "Pandas", "NumPy",
        "Tableau", "Power BI", "Penetration Testing", "SIEM", "Encryption",
        "Continuous Integration", "Continuous Deployment", "CI/CD",

        // Healthcare
        "Patient Care", "Clinical Procedures", "Medication Management", "IV Therapy",
        "Infection Control", "Critical Care", "Physical Therapy", "Occupational Therapy",
        "Speech Therapy", "Electronic Health Records", "Telemedicine Platforms",
        "Epidemiology", "Health Education", "Vaccination Programs",

        // Finance
        "Financial Analysis", "Budgeting", "Forecasting", "Variance Analysis",
        "Portfolio Management", "Risk Analysis", "Asset Allocation",
        "Taxation", "Auditing", "Cost Accounting",
        "Excel", "QuickBooks", "SAP", "Bloomberg Terminal",
        "Anti-Money Laundering", "Know Your Customer", "Blockchain", "Cryptocurrency",

        // Education
        "Curriculum Development", "Lesson Planning", "Assessment Design",
        "Learning Management Systems", "E-Learning Platforms",
        "Special Education", "Individualized Education Programs", "Behavior Management",

        // Engineering
        "CAD", "AutoCAD", "SolidWorks", "3D Printing", "FEA Analysis",
        "Structural Analysis", "Surveying", "BIM", "Circuit Design",
        "PLC Programming", "SCADA Systems", "Flight Dynamics",
        "Avionics Systems", "Propulsion",

        // Marketing
        "SEO", "SEM", "Social Media Marketing", "Email Marketing",
        "Google Analytics", "A/B Testing", "CRM Platforms",
        "Copywriting", "Video Editing", "Graphic Design",
        "Brand Strategy", "Market Research", "Product Positioning",

        // Customer Service
        "CRM Software", "Complaint Resolution", "Inbound Calling", "Outbound Calling",
        "Lead Generation", "Cross-Selling", "Upselling",

        // Legal
        "Legal Research", "Contract Drafting", "Discovery", "Deposition", "Trial Preparation",

        // Construction
        "Construction Management", "Cost Estimation", "Safety Compliance",
        "Heavy Machinery Operation", "Blueprint Reading",

        // Creative/Design
        "Adobe Photoshop", "Illustrator", "InDesign", "Figma", "Sketch",
        "Wireframing", "Prototyping", "Adobe Premiere Pro", "Final Cut Pro",
        "After Effects", "Content Writing", "Technical Writing", "Creative Writing",

        // Soft Skills
        "Verbal Communication", "Written Communication", "Active Listening",
        "Presentation Skills", "Negotiation Skills",
        "Team Management", "Conflict Resolution", "Strategic Planning",
        "Decision-Making", "Coaching", "Mentoring",
        "Critical Thinking", "Analytical Reasoning", "Troubleshooting",
        "Creative Thinking", "Root Cause Analysis", "Prioritization",
        "Goal Setting", "Multitasking", "Scheduling",
        "Collaboration", "Emotional Intelligence", "Adaptability",
        "Dependability", "Open-Mindedness", "Learning New Tools",
        "Resilience", "Flexibility", "Empathy", "Building Trust",

        // Multi-Industry
        "Project Management", "Agile", "Scrum", "Kanban", "Waterfall",
        "JIRA", "Trello", "Asana", "Microsoft Project",
        "Resource Management", "Risk Assessment", "Stakeholder Communication",
        "Statistical Analysis", "Data Visualization", "Predictive Modeling",
        "Business Intelligence", "Office Management", "Data Entry",
        "Microsoft Office Suite", "Record Keeping", "Scheduling"
    };
        // 查询未分析过的描述
        var jobsToAnalyze = await _context.Jobs
            .Where(j => !string.IsNullOrEmpty(j.Description) &&
                        !_context.JobSkills.Any(js => js.JobId == j.Id) // 筛选 JobId 不在 JobSkills 表中的记录
                        && j.Description != "No description available")

            .Take(100) // 每次分析最多 100 条记录
            .ToListAsync();

        if (!jobsToAnalyze.Any())
        {
            Console.WriteLine("No jobs to analyze.");
            return;
        }

        foreach (var job in jobsToAnalyze)
        {
            // 提取技能关键词
            var matchedSkills = skillKeywords
                .Where(skill => Regex.IsMatch(job.Description, $@"\b{skill}\b", RegexOptions.IgnoreCase))
                .ToList();

            if (!matchedSkills.Any())
            {
                _context.JobSkills.Add(new JobSkill
                {
                    JobId = job.Id,
                    Skill = "No skills found",
                    AnalyzedAt = DateTime.UtcNow
                });
                continue;
            }

            // 保存到 JobSkill 表
            foreach (var skill in matchedSkills)
            {
                if (!_context.JobSkills.Any(js => js.JobId == job.Id && js.Skill == skill))
                {
                    _context.JobSkills.Add(new JobSkill
                    {
                        JobId = job.Id,
                        Skill = skill,
                        AnalyzedAt = DateTime.UtcNow
                    });
                }
            }

            Console.WriteLine($"Analyzed Job ID: {job.Id}, Skills: {string.Join(", ", matchedSkills)}");
        }

        await _context.SaveChangesAsync();
        Console.WriteLine("Finished AnalyzeJobSkillsTask.");
    }
}
