import axios from 'axios';
import { Job, SkillData } from '../types';

const API_BASE_URL = 'http://localhost:5182/api'; // 后端服务的地址

// 获取职位列表
export const fetchJobs = async (keyword: string, location: string, page: number = 1): Promise<{
  jobs: Job[];
  totalPages: number;
}> => {
  const response = await axios.get(`${API_BASE_URL}/JobSearch`, {
    params: { keyword, location, page, site: 'seek'},
  });
  return response.data;
};

// 获取技能数据
export const fetchSkillsData = async (): Promise<SkillData[]> => {
  const response = await axios.get(`${API_BASE_URL}/skills`);
  return response.data;
};
