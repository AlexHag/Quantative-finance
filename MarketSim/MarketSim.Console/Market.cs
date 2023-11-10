

using System.Text.Json;
using System.Text.Json.Serialization;

public class Market
{
    private readonly StockService _stockSerivce;

    public Market(StockService stockSerivce)
    {
        _stockSerivce = stockSerivce;
    }

    public void Start()
    {
        while (true)
        {
            GetAction();
        }
    }

    public void GetAction()
    {
        Console.WriteLine("Options:\n\tsell - 1\n\tbuy - 2\n\tprint sell orders - 3\n\tprint buy orders - 4\n\tprint database - 5\n\tprint price evolution - 6");
        Console.Write("Enter: ");
        var action = Console.ReadLine();

        switch (action)
        {
            case "1":
                CreateSellOrder();
                break;
            case "2":
                CreateBuyOrder();
                break;
            case "3":
                PrintSellOrders();
                break;
            case "4":
                PrintBuyOrders();
                break;
            case "5":
                Console.WriteLine(JsonSerializer.Serialize(_stockSerivce._dbContext, new JsonSerializerOptions{ WriteIndented = true, Converters = { new JsonStringEnumConverter() }}));
                break;
            case "6":
                PrintStockPriceEvolution();
                break;
            default:
                Console.WriteLine("Invalid option...");
                GetAction();
                break;
        }
    }

    private void CreateSellOrder()
    {
        Console.Write("UserId: ");
        var userId = int.Parse(Console.ReadLine()!);
        Console.Write("Stock ticker: ");
        var stockTicker = Console.ReadLine();
        var stock = _stockSerivce._dbContext.Stocks.SingleOrDefault(p => p.Ticker == stockTicker);
        if (stock is null)
        {
            throw new Exception("Stock doesn't exist");
        }
        Console.Write("Quantity: ");
        var quantity = int.Parse(Console.ReadLine()!);
        Console.Write("Sell price: ");
        var sellPrice = double.Parse(Console.ReadLine());
        var sellOrder = new StockSellOrder
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            StockId = stock.Id,
            Quantity = quantity,
            QuantityFilled = 0,
            SellPrice = sellPrice,
            Status = StockOrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            FulfilledAt = null
        };
        _stockSerivce.HandleSellOrder(sellOrder);
    }

    private void CreateBuyOrder()
    {
        Console.Write("UserId: ");
        var userId = int.Parse(Console.ReadLine()!);
        Console.Write("Stock ticker: ");
        var stockTicker = Console.ReadLine();
        var stock = _stockSerivce._dbContext.Stocks.SingleOrDefault(p => p.Ticker == stockTicker);
        if (stock is null)
        {
            throw new Exception("Stock doesn't exist");
        }
        Console.Write("Quantity: ");
        var quantity = int.Parse(Console.ReadLine()!);
        Console.Write("Buy price: ");
        var BuyPrice = double.Parse(Console.ReadLine());
        var buyOrder = new StockBuyOrder
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            StockId = stock.Id,
            Quantity = quantity,
            QuantityFilled = 0,
            BuyPrice = BuyPrice,
            Status = StockOrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            FulfilledAt = null
        };
        _stockSerivce.HandleBuyOrder(buyOrder);   
    }

    private void PrintSellOrders()
    {
        var sellOrders = _stockSerivce._dbContext.StockSellOrders.Where(p => p.Status == StockOrderStatus.Pending || p.Status == StockOrderStatus.PartiallyFulfilled).ToList();
        Console.WriteLine(JsonSerializer.Serialize(sellOrders, new JsonSerializerOptions{ WriteIndented = true, Converters = { new JsonStringEnumConverter() }}));
    }

    private void PrintBuyOrders()
    {
        var buyOrders = _stockSerivce._dbContext.StockBuyOrders.Where(p => p.Status == StockOrderStatus.Pending || p.Status == StockOrderStatus.PartiallyFulfilled).ToList();
        Console.WriteLine(JsonSerializer.Serialize(buyOrders, new JsonSerializerOptions{ WriteIndented = true, Converters = { new JsonStringEnumConverter() }}));
    }

    private void PrintStockPriceEvolution()
    {
        Console.Write("Ticker: ");
        
        var ticker = Console.ReadLine();
        var stock = _stockSerivce._dbContext.Stocks.SingleOrDefault(p => p.Ticker == ticker);
        if (stock is null)
            throw new Exception("Stock not found");
        
        var prices = _stockSerivce._dbContext.StockTransactions
            .Where(p => p.StockId == stock.Id)
            .OrderBy(p => p.CreatedAt)
            .Select(p => new StockPrice { Date = p.CreatedAt, Price = p.TradedAt })
            .ToList();
        
        foreach (var price in prices)
        {
            Console.WriteLine($"Date: {price.Date}\t-\tPrice: {price.Price}");
        }
    }
}