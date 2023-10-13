using Microsoft.AspNetCore.Mvc;
using MarketSim.Core.Database;
using MarketSim.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketSim.Core.Controllers;

[ApiController]
[Route("[controller]")]
public class StockController : ControllerBase
{
    private readonly ILogger<StockController> _logger;
    private readonly AppDbContext _dbContext;
    private readonly SystemSettings _system;

    public StockController(ILogger<StockController> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
        _system = _dbContext.SystemSettings.FirstOrDefault()!;
    }

    [HttpGet("{ticker}")]
    public async Task<IActionResult> GetStockPrice(string ticker)
    {
        try
        {
            var stock = _dbContext.Stocks
                .Where(p => p.Ticker == ticker)
                .Include(p => p.StockPrices)
                .FirstOrDefault();
            
            if (stock is null) return NotFound();

            var returns = stock.StockPrices.Where(p => p.Date <= _system.CurrentDay).OrderBy(p => p.Date).ToList();

            return Ok(new { stock.Name, stock.Ticker, returns, price = returns.Last() });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stocks = await _dbContext.Stocks
                .Include(p => p.StockPrices)
                .ToListAsync();
        
        var response = new List<object>();

        foreach(var stock in stocks)
        {
            var price = stock.StockPrices.Where(p => p.Date == _system.CurrentDay).FirstOrDefault();
            response.Add(new { stock.Name, stock.Ticker, price });
        }
        
        return Ok(response);
    }
}
