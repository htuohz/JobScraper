import React from "react";
import { Link } from "react-router-dom";
import Card from "../shared/Card";

const Home: React.FC = () => {
  return (
    <>
      <h2>Dashboard</h2>
      <div style={{ display: "flex", gap: "20px", alignItems: "stretch" }}>
        <Link
          to="/top-locations"
          style={{ textDecoration: "none", flexGrow: 1 }}
        >
          <Card title="Top Locations" />
        </Link>
        <Link to="/top-skills" style={{ textDecoration: "none", flexGrow: 1 }}>
          <Card title="Top Skills" />
        </Link>
      </div>
    </>
  );
};

export default Home;
