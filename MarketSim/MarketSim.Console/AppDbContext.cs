public class AppDbContext
{
    public List<Stock> Stocks { get; set; }
    public List<StockSellOrder> StockSellOrders { get; set; }
    public List<StockBuyOrder> StockBuyOrders { get; set; }
    public List<StockTransaction> StockTransactions { get; set; }
    public List<StockPosition> StockPositions { get; set; }

    public AppDbContext()
    {
        var apple = new Stock
        {
            Id = 1,
            Name = "Apple",
            Ticker = "AAPL",
            Description = "Tech company",
        };
        Stocks = new List<Stock>() { apple };

        // var seller = new StockSellOrder
        // {
        //     Id = Guid.NewGuid(),
        //     StockId = apple.Id,
        //     UserId = 1,
        //     Quantity = 23,
        //     QuantityFilled = 0,
        //     SellPrice = 200,
        //     Status = StockOrderStatus.Pending,
        //     CreatedAt = DateTime.UtcNow,
        //     FulfilledAt = null
        // };
        StockSellOrders = new List<StockSellOrder>(); // { seller };

        StockBuyOrders = new List<StockBuyOrder>()
        {
            new StockBuyOrder
            {
                Id = Guid.NewGuid(),
                StockId = apple.Id,
                UserId = 2,
                Quantity = 5,
                QuantityFilled = 0,
                BuyPrice = 180,
                Status = StockOrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                FulfilledAt = null
            },
            new StockBuyOrder
            {
                Id = Guid.NewGuid(),
                StockId = apple.Id,
                UserId = 3,
                Quantity = 10,
                QuantityFilled = 0,
                BuyPrice = 190,
                Status = StockOrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                FulfilledAt = null
            },
            new StockBuyOrder
            {
                Id = Guid.NewGuid(),
                StockId = apple.Id,
                UserId = 4,
                Quantity = 10,
                QuantityFilled = 0,
                BuyPrice = 200,
                Status = StockOrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                FulfilledAt = null
            },
        };
        
        StockTransactions = new List<StockTransaction>();
        StockPositions = new List<StockPosition>()
        {
            new StockPosition
            {
                Id = Guid.NewGuid(),
                UserId = 1, // seller.UserId,
                StockId = apple.Id,
                Quantity = 50
            }
        };
    }
}