import React from 'react';
import { Bar } from 'react-chartjs-2';
import { SkillData } from '../types';

interface ChartsDashboardProps {
  skillsData: SkillData[];
}

const ChartsDashboard: React.FC<ChartsDashboardProps> = ({ skillsData }) => {
  const chartData = {
    labels: skillsData.map(data => data.skill),
    datasets: [
      {
        label: 'Skill Frequency',
        data: skillsData.map(data => data.frequency),
        backgroundColor: 'rgba(75, 192, 192, 0.6)',
      },
    ],
  };

  return (
    <div>
      <h3>Skill Distribution</h3>
      <Bar data={chartData} />
    </div>
  );
};

export default ChartsDashboard;
