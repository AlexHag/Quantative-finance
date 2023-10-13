import { useNavigate } from "react-router-dom";
import { formatNum } from "./Home";

function StockCard(props) {
  const navigate = useNavigate();

  return (<div style={{
      border: '1px solid black',
      padding: '2%',
      margin: '1%',
      boxShadow: 'rgba(149, 157, 165, 0.2) 0px 8px 24px'
    }} onClick={() => navigate(`/stock/${props.position.stock.ticker}`)}>
    <h2>{props.position.stock.name}</h2>
    <h3>{props.position.stock.ticker}</h3>
    <p>You have: {props.position.quantity}</p>
    <p>Position value: {formatNum(props.position.positionValue.toFixed(2))}</p>
  </div>)
}

export default StockCard;