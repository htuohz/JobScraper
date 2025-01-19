import axios from 'axios';
import { JobSearchResult, LocationResult, SkillData } from '../types';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL; // 后端服务的地址

// 获取职位列表
export const fetchJobs = async (keyword: string, location: string, page: number = 1): Promise<JobSearchResult> => {
  const response = await axios.get(`${API_BASE_URL}/JobSearch`, {
    params: { keyword, location, page, site: 'seek'},
  });
  return response.data;
};

// 获取技能数据
export const fetchTopSkills = async (): Promise<SkillData[]> => {
  const response = await axios.get(`${API_BASE_URL}/Skills/top-skills`);
  return response.data;
};

export const fetchSkillsByJob = async (jobTitle: string, location: string, limit: number): Promise<SkillData[]> => {
  const response = await axios.get(`${API_BASE_URL}/Skills/getSkillsByJobTitle`, {
    params: { jobTitle, location, limit },
  });
  return response.data;
}

export const fetchTopLocations = async (jobTitle: string, limit: number): Promise<LocationResult[]> => {
  const response = await axios.get(`${API_BASE_URL}/JobSearch/top-locations`, {
    params: { jobTitle, limit },
  });
  return response.data;
};