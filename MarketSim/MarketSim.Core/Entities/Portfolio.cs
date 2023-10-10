namespace MarketSim.Core.Entities;

public class Portfolio
{
    public int Id { get; set; }
    public List<CashTransaction> CashTransactions { get; set; } = new List<CashTransaction>();
    public List<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();

    public List<PortfolioPosition> PortfolioPositions { get; set; } = new List<PortfolioPosition>();
    public List<PortfolioReturns> PortfolioReturns { get; set; } = new List<PortfolioReturns>();

    public double CashBalance { get; set; }

    // public List<PortfolioReturns> CalculateReturns(DateTime from)
    // {
    //     var _portfolioReturns = new List<PortfolioReturns>();
    //     var _portfolioPosition = new List<PortfolioPosition>();

    //     var start = StockTransactions
    //         .OrderBy(p => p.CreatedAt).FirstOrDefault()?.CreatedAt ?? throw new Exception("Portfolio has no stock transactions");
        
    //     var interval = from - start;
    //     for (int i = 0; i < interval.Days; i++)
    //     {
    //         var today = start.AddDays(i);
    //         if (today.DayOfWeek == DayOfWeek.Saturday || today.DayOfWeek == DayOfWeek.Sunday)
    //             continue;
            
    //         var stockPurchasesToday = StockTransactions
    //             .Where(p => p.CreatedAt == today)
    //             .Where(p => p.Type == StockTransactionType.Buy)
    //             .ToList();
            
    //         var stockSellsToday = StockTransactions
    //             .Where(p => p.CreatedAt == today)
    //             .Where(p => p.Type == StockTransactionType.Sell)
    //             .ToList();
            
            
    //     }
    // }
}

public class PortfolioPosition
{
    public int Id { get; set; }
    public required Stock Stock { get; set; }
    public required int Quantity { get; set; }
    public double CashIn { get; set; }
    public double CashOut { get; set; }
}

public class PortfolioReturns
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public double TotalValue { get; set; }
}
