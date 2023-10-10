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
        var portfolio = await _dbContext.Portfolios.AddAsync(new Portfolio());
        await _dbContext.SaveChangesAsync();
        return Ok(portfolio.Entity);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPorfolio(int id)
    {
        var portfolio = (await _dbContext.Portfolios
            .Include(p => p.CashTransactions)
            .Include(p => p.StockTransactions)
            .Include(p => p.PortfolioPositions)
                .ThenInclude(p => p.Stock)
            .FirstOrDefaultAsync(p => p.Id == id)) ?? throw new Exception("No portfolio found");

        foreach (var position in portfolio.PortfolioPositions)
        {
            Console.WriteLine(position.Stock.Name);
        }

        return Ok(portfolio);
    }
}
