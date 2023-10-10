using Microsoft.AspNetCore.Mvc;
using MarketSim.Core.Database;
using MarketSim.Core.Services;
using MarketSim.Core.Requests;

namespace MarketSim.Core.Controllers;

[ApiController]
[Route("[controller]")]
public class StockTransactionController : ControllerBase
{
    private readonly ILogger<StockTransactionController> _logger;
    private readonly AppDbContext _dbContext;
    private readonly IStockTransactionService _transactionService;

    public StockTransactionController(
        ILogger<StockTransactionController> logger,
        AppDbContext dbContext,
        IStockTransactionService transactionService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _transactionService = transactionService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction(CreateStockTransactionRequest request)
    {
        var transaction = await _transactionService.CreateStockTransaction(request);
        return Ok(transaction);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransaction(int id)
    {
        var transaction = await _dbContext.CashTransactions.FindAsync(id);
        return Ok(transaction);
    }
}
