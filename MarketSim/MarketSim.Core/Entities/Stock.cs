using MarketSim.Core.Exceptions;

namespace MarketSim.Core.Entities;

public class Stock
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Ticker { get; set; }
    public List<StockPrice> StockPrices { get; set; } = new List<StockPrice>();

    public double GetCurrentPrice(DateTime date)
    {
        var price = StockPrices.Where(p => p.Date == date).FirstOrDefault();
        if (price is null) throw new PriceNotFoundExceptionException($"Could not find price today: {date} for stock {Name}");
        return price.Open;
    }
}

public class StockPrice
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public double Open { get; set; }
    public double Close { get; set; }
}
