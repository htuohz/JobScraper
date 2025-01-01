import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import HomePage from "./pages/Home";
import TopLocationsPage from "./pages/TopLocationsPage";
import Navbar from "./components/navbar/NavBar"; // Import the Navbar component
import TopSkillsPage from "./pages/TopSkillsPage";

const App: React.FC = () => {
  return (
    <Router>
      <Navbar /> {/* Include the navigation bar */}
      <div className="container">
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/top-skills" element={<TopSkillsPage />} />
          <Route path="/top-locations" element={<TopLocationsPage />} />
        </Routes>
      </div>
    </Router>
  );
};

export default App;
