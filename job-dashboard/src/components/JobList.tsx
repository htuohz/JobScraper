import React from "react";
import { Job } from "../types";
import Spinner from "../shared/Spinner";

interface JobListProps {
  jobs: Job[];
  isLoading: boolean;
}

const JobList: React.FC<JobListProps> = ({ jobs, isLoading }) => {
  if (isLoading) {
    return <Spinner />;
  }
  return (
    <div>
      {jobs?.map((job, index) => (
        <div
          key={index}
          style={{ borderBottom: "1px solid #ccc", padding: "10px" }}
        >
          <a
            href={job.url}
            target="_blank"
            rel="noopener noreferrer"
            style={{ fontWeight: "bold" }}
          >
            {job.title}
          </a>
          <p>
            {job.company} - {job.location}
          </p>
          <p>{job.description}</p>
          <p style={{ color: "gray", fontSize: "12px" }}>
            Posted: {job.postedDate}
          </p>
        </div>
      ))}
    </div>
  );
};

export default JobList;
