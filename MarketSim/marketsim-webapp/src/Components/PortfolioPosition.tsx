import { useState, useEffect } from "react";
import { PortfolioPosition } from "../Interfaces";

function PortfolioPositionCard(position: PortfolioPosition) {
  const [stockPrice, setStockPrice] = useState('');

  useEffect(() => {
    getStockPrice(position.stock?.ticker).then(p => setStockPrice(p));
  }, [])

  const getStockPrice = async (ticker: string | undefined) => {
    if (ticker == undefined) return '0';
    var response = await fetch(`http://localhost:5234/Stock/${ticker}`);
    return response.json();
  }

  return (<div className='card'>
    <h2>{position.stock?.name}</h2>
    <h3>{position.stock?.ticker}</h3>
    <p>You have: {position.quantity}</p>
    <p>Today price: {stockPrice}</p>
  </ div>)
}

export default PortfolioPositionCard;