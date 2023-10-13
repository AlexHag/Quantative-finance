import { useState, useEffect } from "react";
import './button.css'

function TradeStock(props) {
  // const [stocks, setStocks] = useState();
  
  // const [type, setType] = useState("Buy");
  // const [ticker, setTicker] = useState('');
  // const [quantity, setQuantity] = useState();
  // const [error, setError] = useState('');

  // const fetchStocks = async () => {
  //   var response = await fetch('http://localhost:5234/Stock');
  //   var data = await response.json();
  //   setStocks(data);
  // }

  // useEffect(() => {
  //   fetchStocks();
  // }, [])

  // const submit = async () => {
  //   const requestOptions = {
  //     method: 'POST',
  //     headers: { 'Accept': 'application/json',
  //               'Content-Type': 'application/json' },
  //     body: JSON.stringify({ portfolioId: props.portfolioId, ticker, type, quantity })
  //   };

  //   const response = await fetch(`http://localhost:5234/StockTransaction`, requestOptions);
  // }

  const buyStock = async (ticker) => {
    const requestOptions = {
      method: 'POST',
      headers: { 'Accept': 'application/json',
                'Content-Type': 'application/json' },
      body: JSON.stringify(
        {
          portfolioId: props.portfolioId, 
          ticker: ticker,
          type: "Buy", 
          quantity: 1 
        })
    };

    await fetch(`http://localhost:5234/StockTransaction`, requestOptions);
    await props.fetchData();
  }

  const sellStock = async (ticker) => {
    const requestOptions = {
      method: 'POST',
      headers: { 'Accept': 'application/json',
                'Content-Type': 'application/json' },
      body: JSON.stringify(
        {
          portfolioId: props.portfolioId, 
          ticker: ticker,
          type: "Sell", 
          quantity: 5
        })
    };

    await fetch(`http://localhost:5234/StockTransaction`, requestOptions);
    await props.fetchData();
  }

  return (<>
    {/* <h2>Trade</h2>
    <select onChange={(e) => setType(e.target.value)}>
        <option value="Buy">Buy</option>
        <option value="Sell">Sell</option>
    </select>

    <input
      type="text"
      value={ticker}
      onChange={(e) => setTicker(e.target.value)}
      placeholder='Ticker'
      />

      <input
      type="number"
      value={quantity}
      onChange={(e) => setQuantity(Number.parseInt(e.target.value))}
      placeholder='Quantity'
      />

    <button onClick={submit}>Submit</button>

    <br />
    <br /> */}
    {props.stocks && 
    <div className="stock-board"> {props.stocks.map(p => 
      <div className="stock-board-row">
        <p className="stock-board-row-item"><b>{p.name}</b> ({p.ticker})</p>
        <p className="stock-board-row-item">{p.price.open}</p>
        <button className="stock-board-row-item button-48" onClick={() => buyStock(p.ticker)}>BUY</button>
        <button className="stock-board-row-item button-48" onClick={() => sellStock(p.ticker)}>SELL</button>
      </div>)} 
    </div> }
  </>)
}

export default TradeStock;