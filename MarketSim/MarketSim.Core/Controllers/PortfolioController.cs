using Microsoft.AspNetCore.Mvc;
using MarketSim.Core.Database;
using MarketSim.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketSim.Core.Controllers;

[ApiController]
[Route("[controller]")]
public class PortfolioController : ControllerBase
{
    private readonly ILogger<PortfolioController> _logger;
    private readonly AppDbContext _dbContext;

    public PortfolioController(ILogger<PortfolioController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePortfolio()
    {
        try
        {
            var portfolio = await _dbContext.Portfolios.AddAsync(new Portfolio());
            await _dbContext.SaveChangesAsync();
            return Ok(portfolio.Entity);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPorfolio(int id, bool includeTransactions)
    {
        try
        {

            var portfolio = (await _dbContext.Portfolios
                // .Include(p => p.CashTransactions)
                // .Include(p => p.StockTransactions)
                .Include(p => p.PortfolioReturns)
                .Include(p => p.PortfolioPositions)
                    .ThenInclude(p => p.Stock)
                .FirstOrDefaultAsync(p => p.Id == id)) ?? throw new Exception("No portfolio found");

            if (includeTransactions)
            {
                _dbContext.Entry(portfolio)
                .Collection(p => p.CashTransactions)
                .Load();
                _dbContext.Entry(portfolio)
                .Collection(p => p.StockTransactions)
                .Load();
            }

            foreach (var position in portfolio.PortfolioPositions)
            {
                Console.WriteLine(position.Stock.Name);
            }

            return Ok(portfolio);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
