import React from "react";

interface SelectProps {
  options: { value: string | number; label: string }[];
  onChange: (event: React.ChangeEvent<HTMLSelectElement>) => void;
  title?: string;
  value?: string | number;
}

const Select = ({ options, onChange, title }: SelectProps) => {
  return (
    <div style={{display:'flex', padding: 2}}>
      <select onChange={onChange}>
        <option value="" disabled selected>
          {title}
        </option>
        {options.map((option) => (
          <option key={option.value} value={option.value}>
            {option.label}
          </option>
        ))}
      </select>
    </div>
  );
};

export default Select;
