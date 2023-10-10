using MarketSim.Core.Entities;

namespace MarketSim.Core.Requests;

public class CreateStockTransactionRequest
{
    public int PortfolioId { get; set; }
    public required string Ticker { get; set; }
    public StockTransactionType Type { get; set; }
    public int Quantity { get; set; }
}