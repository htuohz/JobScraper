import React, { useCallback, useEffect, useState } from "react";
import SearchBar from "../components/SearchBar";
import JobList from "../components/JobList";
import Pagination from "../components/Pagination";
import { fetchJobs } from "../services/jobService";
import { Job } from "../types";

const Home: React.FC = () => {
  const [jobs, setJobs] = useState<Job[]>([]);
  const [keyword, setKeyword] = useState("");
  const [location, setLocation] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1); // 动态总页数

  // 搜索和分页触发的核心逻辑
  const loadJobs = useCallback(async () => {
    if (!keyword && !location) return; // 防止无搜索条件触发
    setIsLoading(true);
    try {
      // 假设 API 返回 jobs 和 totalPages
      const { jobs: fetchedJobs, totalPages: fetchedTotalPages } = await fetchJobs(
        keyword,
        location,
        currentPage
      );
      setJobs(fetchedJobs);
      setTotalPages(fetchedTotalPages || 1); // 动态设置总页数
    } catch (err) {
      console.log(err);
    } finally {
      setIsLoading(false);
    }
  }, [keyword, location, currentPage]);

  // 监听分页变化和搜索条件
  useEffect(() => {
    loadJobs();
  }, [loadJobs]);

  // 搜索触发逻辑
  const handleSearch = (searchKeyword: string, searchLocation: string) => {
    setKeyword(searchKeyword);
    setLocation(searchLocation);
    setCurrentPage(1); // 每次新搜索回到第一页
  };

  // 分页切换
  const handlePageChange = (page: number) => {
    setCurrentPage(page);
  };

  return (
    <div className="container">
      <h1>Job Dashboard</h1>
      <SearchBar onSearch={handleSearch} disabled={isLoading} />
      <JobList jobs={jobs} isLoading={isLoading} />
      <Pagination
        currentPage={currentPage}
        totalPages={totalPages}
        onPageChange={handlePageChange}
      />
    </div>
  );
};

export default Home;
