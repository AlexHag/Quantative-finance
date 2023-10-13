import { useState, useEffect } from "react";
import { SparkLineChart } from '@mui/x-charts';
import TradeStock from "../Components/TradeStock";
import StockCard from "./StockCard";

export function formatNum(x) {
  if (!x) return 0;
  var parts = x.toString().split(".");
  parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, " ");
  return parts.join(".");
}

function Home() {
  const [portfolioId, setPortfolioId] = useState(1);
  window.setPortfolioId = setPortfolioId;
  const [portfolio, setPortfolio] = useState();
  const [stocks, setStocks] = useState();

  const fetchPortfolio = async () => {
    var response = await fetch(`http://localhost:5234/Portfolio/${portfolioId}`);
    const data = await response.json();
    setPortfolio(data);
  }

  const fetchStocks = async () => {
    var response = await fetch('http://localhost:5234/Stock');
    var data = await response.json();
    setStocks(data);
  }

  const fetchData = async () => {
    await fetchPortfolio();
    await fetchStocks();
  }

  const nextDay = async () => {
    await fetch('http://localhost:5234/System/AddDay');
    await fetchData();
  }

  const doStuff = () => {
    console.log("bruh");
    console.log(portfolio.portfolioReturns.slice(-1)[0].date.split("T")[0]);
  }

  return (
    <>
      <h1 style={{fontSize: '50px'}}>Market Sim</h1>
      {portfolio && <>

        <h1>Avaliable for buy: ${formatNum(portfolio.cashBalance.toFixed(2))}</h1>
        <h2>Todays date: {portfolio.portfolioReturns.slice(-1)[0].date.split("T")[0]}</h2>
      </>}
      
      <br></br>

      <TradeStock stocks={stocks} portfolioId={portfolioId} fetchData={fetchData} />

      {portfolio && <>
        <br />
        <br />
        
        {portfolio.portfolioReturns.length != 0 && <div style={{maxWidth: '50%', margin: 'auto'}}>
          <SparkLineChart
              plotType="line"
              data={portfolio.portfolioReturns.map(p => p.value)}
              height={200}
          />
        </div>}

        {portfolio.portfolioPositions.length != 0 && 
        <div style={{display: 'flex', justifyContent: 'center'}}>
          {portfolio.portfolioPositions.map(p => <StockCard position={p} />)}
        </div>}

      </>}
      <button onClick={fetchData}>Fetch data</button>
      <button onClick={nextDay} >Next Day</button>
      <button onClick={doStuff}>Do stuff</button>
    </>)
}

export default Home;