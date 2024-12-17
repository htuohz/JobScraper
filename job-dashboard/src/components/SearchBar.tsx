import React, { useState } from 'react';
import Input from '../shared/Input';
import Button from '../shared/Button';

interface SearchBarProps {
  onSearch: (keyword: string, location: string) => void;
  disabled?: boolean;
}

const SearchBar: React.FC<SearchBarProps> = ({ onSearch, disabled = false }) => {
  const [keyword, setKeyword] = useState('');
  const [location, setLocation] = useState('');

  return (
    <div className="search-bar" style={{ display: 'flex', gap: '10px', marginBottom: '20px' }}>
      <Input
        type="text"
        placeholder="Enter job title or keyword"
        value={keyword}
        onChange={(e) => setKeyword(e.target.value)}
        style={{ padding: '10px', width: '300px' }}
      />
      <Input
        type="text"
        placeholder="Enter location"
        value={location}
        onChange={(e) => setLocation(e.target.value)}
        style={{ padding: '10px', width: '200px' }}
      />
      <Button onClick={() => onSearch(keyword, location)} style={{ padding: '10px 20px' }} disabled={disabled}>
        Search
      </Button>
    </div>
  );
};

export default SearchBar;
