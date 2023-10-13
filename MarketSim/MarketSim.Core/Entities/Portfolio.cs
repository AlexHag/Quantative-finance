using MarketSim.Core.Exceptions;

namespace MarketSim.Core.Entities;

public class Portfolio
{
    public int Id { get; set; }
    public List<CashTransaction> CashTransactions { get; set; } = new List<CashTransaction>();
    public List<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();

    public List<PortfolioPosition> PortfolioPositions { get; set; } = new List<PortfolioPosition>();
    public List<PortfolioReturns> PortfolioReturns { get; set; } = new List<PortfolioReturns>();

    public double CashBalance { get; set; }

    public double CalculateTotalPositionsValue(DateTime date)
    {
        double totalPositionsValue = 0;

        foreach (var position in PortfolioPositions)
        {
            var stockPrice = position.Stock.StockPrices
                .Where(p => p.Date == date)
                .FirstOrDefault()?.Open 
                ?? throw new PriceNotFoundExceptionException($"Could not find price for this stock today: {date}");

            var positionValue = stockPrice * position.Quantity;
            totalPositionsValue += positionValue;
        }

        return totalPositionsValue;
    }
}

public class PortfolioPosition
{
    public int Id { get; set; }
    public required Stock Stock { get; set; }
    public required int Quantity { get; set; }
    public double PositionValue { get; set; }
}

public class PortfolioReturns
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public double Value { get; set; }
}
