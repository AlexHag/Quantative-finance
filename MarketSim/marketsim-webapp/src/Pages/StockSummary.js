import { useParams, useNavigate, Link } from "react-router-dom";
import { useEffect, useState } from "react";
import { SparkLineChart } from '@mui/x-charts';
function StockSummary() {
  let { ticker } = useParams()
  const [stockInfo, setStockInfo] = useState('')
  const [error, setError] = useState()

  useEffect(() => {
    getStock();
  }, [])

  const getStock = async () => {
    var response = await fetch(`http://localhost:5234/Stock/${ticker}`)
    
    if (response.status == 404) {
      setError("Not found")
    }

    var data = await response.json()
    console.log(data)
    setStockInfo(data)
  }

  if (error) return <>{error}</>

  return (<>
  <Link to='/' >Home</Link>
  {stockInfo && <>
    <h1>{stockInfo.name} ({ticker})</h1>
    <h2 style={{marginTop: '2%'}}>Price: {stockInfo.price.open}</h2>
    <div style={{maxWidth: '50%', margin: 'auto', marginTop: '2%', padding: '1%', border: '1px solid black'}}>
      <SparkLineChart
          plotType="line"
          data={stockInfo.returns.map(p => p.open)}
          height={200}
      />
    </div>
  </>}
  </>)
}

export default StockSummary;