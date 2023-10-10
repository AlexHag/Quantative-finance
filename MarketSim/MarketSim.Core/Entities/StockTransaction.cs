namespace MarketSim.Core.Entities;

public class StockTransaction
{
    public int Id { get; set; }
    public required Portfolio Portfolio { get; set; }
    public required Stock Stock { get; set; }
    public int Quantity { get; set; }
    public double OrderPrice { get; set; }
    public StockTransactionType Type { get; set; }
    public DateTime CreatedAt { get; set; }
}

public enum StockTransactionType
{
    Buy = 1,
    Sell = 2,
    Short = 3
}
