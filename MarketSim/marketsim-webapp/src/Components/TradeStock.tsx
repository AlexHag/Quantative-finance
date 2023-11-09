import { Console } from "console";
import { useState } from "react";

const TradeStock: React.FC<{ portfolioId: number }> = ({ portfolioId }) => {
    const [type, setType] = useState("Buy");
    const [ticker, setTicker] = useState('');
    const [quantity, setQuantity] = useState<number>();
    const [error, setError] = useState('');

    const submit = async () => {
        const requestOptions = {
            method: 'POST',
            headers: { 'Accept': 'application/json',
                      'Content-Type': 'application/json' },
            body: JSON.stringify({ portfolioId, ticker, type, quantity })
        };

        const response = await fetch(`http://localhost:5234/StockTransaction`, requestOptions);
    }

    return (<div>
        <h2>Trade</h2>
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
    </div>)
}

export default TradeStock;