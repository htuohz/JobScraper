import React from 'react';

interface ButtonProps {
  onClick: () => void;
  children: React.ReactNode;
  className?: string;
  style?: React.CSSProperties;
  disabled?: boolean;
}

const Button: React.FC<ButtonProps> = ({
  onClick,
  children,
  className,
  style,
  disabled = false,
}) => {
  return (
    <button
      onClick={onClick}
      className={className}
      style={{
        padding: '10px 20px',
        backgroundColor: disabled ? '#ccc' : '#007BFF',
        color: '#fff',
        border: 'none',
        borderRadius: '4px',
        cursor: disabled ? 'not-allowed' : 'pointer',
        ...style,
      }}
      disabled={disabled}
    >
      {children}
    </button>
  );
};

export default Button;
