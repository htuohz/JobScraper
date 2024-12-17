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
    frequency: number;
  }
  
  // IndustryData.ts
  export interface IndustryData {
    industry: string;
    count: number;
  }
  
  // LocationData.ts
  export interface LocationData {
    location: string;
    count: number;
  }
  