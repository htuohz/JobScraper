import React, { useState } from "react";
import Input from "../shared/Input";
import Button from "../shared/Button";
import Select from "../shared/Select";

interface SearchBarProps {
  onSearch: (keyword: string, location: string, limit: number) => void;
  disabled?: boolean;
  type?: SearchType;
}

export enum SearchType {
  SKILL = "skill",
  LOCATION = "location",
}

const SearchBar: React.FC<SearchBarProps> = ({
  onSearch,
  disabled = false,
  type = SearchType.SKILL,
}) => {
  const [keyword, setKeyword] = useState("");
  const [location, setLocation] = useState("");
  const [limit, setLimit] = useState(10);
  const options = [
    { value: 10, label: "10" },
    { value: 20, label: "20" },
    { value: 50, label: "50" },
  ];

  return (
  <>
    <div
      className="search-bar"
      style={{ display: "flex", gap: "10px", marginBottom: "20px" }}
    >
      <Input
        type="text"
        placeholder="Enter job title or keyword"
        value={keyword}
        onChange={(e) => setKeyword(e.target.value)}
        style={{ padding: "10px", width: "300px" }}
      />
      {type !== SearchType.LOCATION && (
        <Input
          type="text"
          placeholder="Enter location"
          value={location}
          onChange={(e) => setLocation(e.target.value)}
          style={{ padding: "10px", width: "200px" }}
        />
      )}
      <Select
        options={options}
        value={limit}
        onChange={(event) => setLimit(Number(event.target.value))}
        title="Select limit"
      />
      <Button
        onClick={() => onSearch(keyword, location, limit)}
        style={{ padding: "10px 20px" }}
        disabled={disabled}
      >
        Search
      </Button>
    </div>
    <p>{`Showing result for ${keyword} ${location && `in ${location}`} with limit of ${limit}`}</p>
    </>
  );
};

export default SearchBar;
