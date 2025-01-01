import React, { useCallback, useState } from "react";
import { fetchSkillsByJob } from "../services/jobService";
import Chart from 'chart.js/auto';
import {CategoryScale, ChartData} from 'chart.js'; 
import SearchBar from "./SearchBar";
import Bar from "../shared/Bar";
Chart.register(CategoryScale);

const TopSkills: React.FC = () => {
  const [chartData, setChartData] = useState<ChartData<'bar'>>({
    labels: [],
    datasets: [],
  });
  const [loading, setLoading] = useState(false);

  const onSearch = useCallback(async (keyword: string, location: string, limit: number = 10) => {
    setLoading(true);
    console.log('limit', limit);
    try{
      const data = await fetchSkillsByJob(keyword, location, limit);
      setChartData({
        labels: data?.map((item) => item.skill),
        datasets: [
          {
            data: data?.map((item) => item.count),
            backgroundColor: "rgba(75, 192, 192, 0.6)",
            label: 'Number of Jobs'
          },
          
        ],
      });
    }finally{
      setLoading(false);
    }
  }, []);


  return (
    <div>
      <h3>Top Skills</h3>
      <SearchBar onSearch={onSearch} />
      <Bar data={chartData} loading={loading}/>
    </div>
  );
};

export default TopSkills;
