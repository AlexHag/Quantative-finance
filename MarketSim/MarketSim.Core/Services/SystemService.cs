using MarketSim.Core.Database;
using MarketSim.Core.Entities;
using Microsoft.EntityFrameworkCore;
using MarketSim.Core.Exceptions;

namespace MarketSim.Core.Services;

public interface ISystemService
{
    Task AddSystemDay();
    Task UpdatePortfolioData(DateTime date);
}

public class SystemService : ISystemService
{
    private readonly AppDbContext _dbContext;
    private readonly SystemSettings _system;

    public SystemService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _system = dbContext.SystemSettings.FirstOrDefault()!;
    }

    public async Task AddSystemDay()
    {
        try
        {
            if (_system.CurrentDay.DayOfWeek == DayOfWeek.Friday)
            {
                _system.CurrentDay = _system.CurrentDay.AddDays(3);
            }
            else if (_system.CurrentDay.DayOfWeek == DayOfWeek.Saturday)
            {
                _system.CurrentDay = _system.CurrentDay.AddDays(2);
            }
            else
            {
                _system.CurrentDay = _system.CurrentDay.AddDays(1);
            }

            await UpdatePortfolioData(_system.CurrentDay);

            await _dbContext.SaveChangesAsync();
        }
        catch (PriceNotFoundExceptionException e)
        {
            Console.WriteLine("Caught price not found exception while adding system day. Adding another day and trying again.");
            _system.CurrentDay = _system.CurrentDay.AddDays(1);
            await AddSystemDay();
        }
    }

    public async Task UpdatePortfolioData(DateTime date)
    {
        var portfolios = await _dbContext.Portfolios
            .Include(p => p.PortfolioPositions)
                .ThenInclude(p => p.Stock)
                    .ThenInclude(p => p.StockPrices)
            .ToListAsync();
        
        foreach (var portfolio in portfolios)
        {
            portfolio.PortfolioReturns.Add(new PortfolioReturns
            {
                Date = date,
                Value = portfolio.CalculateTotalPositionsValue(date)
            });

            foreach (var position in portfolio.PortfolioPositions)
            {
                position.PositionValue = position.Stock.GetCurrentPrice(date) * position.Quantity;
            }
        }

        await _dbContext.SaveChangesAsync();
    }
}
