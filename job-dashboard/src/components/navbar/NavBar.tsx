import React, { useState } from "react";
import { Link } from "react-router-dom";
import "./Navbar.css";

const Navbar: React.FC = () => {
  const [isMobileMenuOpen, setMobileMenuOpen] = useState(false);

  const toggleMobileMenu = () => {
    setMobileMenuOpen(!isMobileMenuOpen);
  };

  return (
    <nav className="navbar">
      <div className="navbar-container">
        {/* Logo */}
        <Link to="/" className="navbar-logo">
          Home
        </Link>

        {/* Desktop Menu */}
        <ul className={`navbar-links ${isMobileMenuOpen ? "active" : ""}`}>
          <li>
            <Link to="/about" className="navbar-link">
              About
            </Link>
          </li>
          <li>
            <Link to="/contact" className="navbar-link">
              Contact
            </Link>
          </li>
        </ul>

        {/* Mobile Menu Toggle */}
        <button className="mobile-menu-icon" onClick={toggleMobileMenu}>
          ☰
        </button>
      </div>
    </nav>
  );
};

export default Navbar;
