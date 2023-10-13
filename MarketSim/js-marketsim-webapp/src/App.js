import './App.css';
import { BrowserRouter, Link, Routes, Route } from 'react-router-dom';
import Home from './Pages/Home';
import StockSummary from './Pages/StockSummary';

function App() {
  return (
    <div className="App">
      <BrowserRouter> 
        <Routes> 
          <Route path="/" element={<Home />}></Route>
          <Route path="Stock/:ticker" element={<StockSummary />}></Route>
        </Routes>  
      </BrowserRouter>
    </div>
  );
}

export default App;
