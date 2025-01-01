import Spinner from "./Spinner";
import Chart from 'chart.js/auto';
import {CategoryScale} from 'chart.js'; 
import { Bar as ChartBar } from "react-chartjs-2";
Chart.register(CategoryScale);

interface BarProps extends React.ComponentProps<typeof ChartBar> {
    loading?: boolean;
}

const Bar: React.FC<BarProps> = ({ loading, ...props }) => {
    return (
        <div style={{ display: 'flex', gap: '10px', marginBottom: '20px' }}>
            {loading ? <Spinner /> : <ChartBar {...props} />}
        </div>
    );
};

export default Bar;