using MarketSim.Core.Database;
using MarketSim.Core.Entities;
using MarketSim.Core.Requests;
using Microsoft.EntityFrameworkCore;

namespace MarketSim.Core.Services;

public interface IStockTransactionService
{
    Task<StockTransaction> CreateStockTransaction(CreateStockTransactionRequest request);
}

public class StockTransactionService : IStockTransactionService
{
    private readonly AppDbContext _dbContext;
    private readonly SystemSettings _system;

    public StockTransactionService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _system = _dbContext.SystemSettings.FirstOrDefault()!;
    }

    public async Task<StockTransaction> CreateStockTransaction(CreateStockTransactionRequest request)
    {
        var portfolio = await _dbContext.Portfolios
            .Include(p => p.PortfolioPositions)
            .Where(p => p.Id == request.PortfolioId)
            .FirstOrDefaultAsync();
        
        if (portfolio is null)
            throw new Exception("Portfolio not found");
        
        return request.Type switch
        {
            StockTransactionType.Buy => await BuyStock(request, portfolio),
            StockTransactionType.Sell => await SellStock(request, portfolio),
            _ => throw new NotImplementedException()
        };
    }


    public async Task<StockTransaction> BuyStock(CreateStockTransactionRequest request, Portfolio portfolio)
    {
        var stock = await _dbContext.Stocks
            .Include(p => p.StockPrices)
            .Where(p => p.Ticker == request.Ticker)
            .FirstOrDefaultAsync();

        if (stock is null)
            throw new Exception($"Could not find stock with ticker {request.Ticker}");

        var stockPrice = stock.StockPrices
            .Where(p => p.Date == _system.CurrentDay)
            .FirstOrDefault()?
            .Open ?? throw new Exception($"Could not find price for this stock today: {_system.CurrentDay}");

        var transaction = new StockTransaction
        {
            Portfolio = portfolio,
            Stock = stock,
            Quantity = request.Quantity,
            OrderPrice = stockPrice * request.Quantity,
            Type = StockTransactionType.Buy,
            CreatedAt = _system.CurrentDay
        };

        if (portfolio.CashBalance < transaction.OrderPrice)
            throw new Exception($"Insufficient balance to purchase {transaction.Quantity} amount of stocks in {stock.Name} for the price of {transaction.OrderPrice} when your balance is {portfolio.CashBalance}");
        
        var portfolioPosition = portfolio.PortfolioPositions
            .Where(p => p.Stock == stock)
            .FirstOrDefault();
        
        if (portfolioPosition is null)
        {
            var newPosition = new PortfolioPosition
            {
                Stock = stock,
                Quantity = transaction.Quantity,
                PositionValue = transaction.OrderPrice
            };

            portfolio.PortfolioPositions.Add(newPosition);
        }
        else
        {
            portfolioPosition.Quantity += transaction.Quantity;
            portfolioPosition.PositionValue += transaction.OrderPrice;
        }

        portfolio.CashBalance -= transaction.OrderPrice;
        portfolio.StockTransactions.Add(transaction);

        await _dbContext.SaveChangesAsync();
        return transaction;
    }


    public async Task<StockTransaction> SellStock(CreateStockTransactionRequest request, Portfolio portfolio)
    {
        var stock = await _dbContext.Stocks
            .Include(p => p.StockPrices)
            .Where(p => p.Ticker == request.Ticker)
            .FirstOrDefaultAsync();

        if (stock is null)
            throw new Exception($"Could not find stock with ticker {request.Ticker}");
        
        var stockPrice = stock.StockPrices
            .Where(p => p.Date == _system.CurrentDay)
            .FirstOrDefault()?
            .Open ?? throw new Exception("Could not find price for this stock today");

        var transaction = new StockTransaction
        {
            Portfolio = portfolio,
            Stock = stock,
            Quantity = request.Quantity,
            OrderPrice = stockPrice * request.Quantity,
            Type = StockTransactionType.Sell,
            CreatedAt = _system.CurrentDay
        };

        var position = portfolio.PortfolioPositions
            .Where(p => p.Stock == stock)
            .FirstOrDefault();

        if (position is null)
            throw new Exception($"Portfolio contain no positions of the stock: {request.Ticker}");
        
        if (position.Quantity < request.Quantity)
        {
            throw new Exception($"You do not have enough of the stock {stock.Name}. You have {position.Quantity} and you requested to sell {request.Quantity}");
        }
        else
        {
           position.Quantity -= request.Quantity;
           position.PositionValue -= transaction.OrderPrice;
        }

        portfolio.CashBalance += transaction.OrderPrice;
        portfolio.StockTransactions.Add(transaction);

        await _dbContext.SaveChangesAsync();

        return transaction;
    }
}
