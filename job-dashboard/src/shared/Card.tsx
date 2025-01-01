import React from "react";

interface CardProps {
  title: string;
}

const Card: React.FC<CardProps> = ({ title }) => {
  return (
    <div
      className="card"
    >
      <h3>{title}</h3>
    </div>
  );
};

export default Card;
