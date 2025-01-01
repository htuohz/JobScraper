// Job.ts
export interface Job {
    title: string;
    company: string;
    location: string;
    postedDate: string;
    url: string;
    description: string;
  }
  
  // SkillData.ts
  export interface SkillData {
    skill: string;
    count: number;
  }
  
export interface JobSearchResult {
    jobs: Job[];
    totalPages: number
}
  