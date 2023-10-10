using MarketSim.Core.Entities;

namespace MarketSim.Core.Requests;

public class CreateCashTransactionRequest
{
    public int PortfolioId { get; set; }
    public CashTransactionType Type { get; set; }
    public double Amount { get; set; }
}