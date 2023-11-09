import React from 'react';
// import logo from './logo.svg';
import './App.css';
// import { useState } from 'react';
// import { Portfolio } from './Interfaces';
// import PortfolioPositionCard from './Components/PortfolioPosition';
// import TradeStock from './Components/TradeStock';
// import SelectDate from './Components/SelectDate';
// import { LineChart, SparkLineChart } from '@mui/x-charts';
// import { Box } from '@mui/material';


import { BrowserRouter, Link, Routes, Route } from 'react-router-dom';
import Home from './Pages/Home';
import StockSummary from './Pages/StockSummary';

function App() {
  // const [portfolioId, setPortfolioId] = useState(1);
  // (window as any).setPortfolioId = setPortfolioId;

  // const [portfolio, setPortfolio] = useState<Portfolio>();
  
  // function formatNum(x: any) {
  //   if (!x) return 0;
  //   var parts = x.toString().split(".");
  //   parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, " ");
  //   return parts.join(".");
  // }

  // const doStuff = () => {
  //   if (portfolio?.portfolioReturns)
  //   {
  //     console.log(portfolio.portfolioReturns.map(returnData => returnData.value).filter(value => value !== undefined) as number[])
  //     console.log("\n");
  //     console.log(portfolio.portfolioReturns.map(returnData => returnData.date));
  //   }
  // }

  // const refreshPortfolio = async () => {
  //   var response = await fetch(`http://localhost:5234/Portfolio/${portfolioId}`);
  //   const data: Portfolio = await response.json();
  //   setPortfolio(data);
  // }

  return (
    <div className="App">
      <BrowserRouter> 
        <Routes> 
          <Route path="/" element={<Home />}></Route>

          <Route path="Stock/:ticker" element={<StockSummary />}></Route>

        </Routes>  
      </BrowserRouter>

      {/* <h1 style={{fontSize: '50px'}}>Market Sim</h1>
      <h1>Avaliable for buy: ${formatNum(portfolio?.cashBalance?.toFixed(2))}</h1>
      
      <SelectDate />
      <TradeStock portfolioId={portfolioId} />
      
      <br></br>
      <br></br>

      <div style={{maxWidth: '50%', margin: 'auto'}}>
        {portfolio?.portfolioReturns && 
          <SparkLineChart
            plotType="line"
            data={portfolio.portfolioReturns.map(p => p.value) as number[]}
            height={200}
          />
        }
      </div>
      
      <br></br>
      <div className='hello'>
        {portfolio?.portfolioPositions?.map(p => <PortfolioPositionCard {...p} />)}
      </div>

      <button onClick={refreshPortfolio}>Refresh portfolio</button>
      <button onClick={doStuff}>Do stuff</button> */}
    </div>
  );
}

export default App;
