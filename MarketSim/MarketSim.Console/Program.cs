public class Program
{
    public static void Main(string[] args)
    {
        var dbContext = new AppDbContext();        
        var stockService = new StockService(dbContext);
        var market = new Market(stockService);

        market.Start();
    }
}
