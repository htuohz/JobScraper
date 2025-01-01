import React, { useCallback, useState } from "react";
import { fetchTopLocations } from "../services/jobService";
import Chart from 'chart.js/auto';
import {CategoryScale, ChartData} from 'chart.js'; 
import SearchBar, { SearchType } from "./SearchBar";
import Bar from "../shared/Bar";
Chart.register(CategoryScale);

const TopLocations: React.FC = () => {
  const [chartData, setChartData] = useState<ChartData<'bar'>>({
    labels: [],
    datasets: [],
  });
  const [loading, setLoading] = useState(false);

  const onSearch = useCallback(async (jobTitle: string, _location: string, limit: number = 10) => {
    setLoading(true);
    try{
      const data = await fetchTopLocations(jobTitle, limit);
      setChartData({
        labels: data?.map((item) => item.location),
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
      <h3>Top Locations</h3>
      <SearchBar onSearch={onSearch} type={SearchType.LOCATION}/>
      <Bar data={chartData} loading={loading}/>
    </div>
  );
};

export default TopLocations;
