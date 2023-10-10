using Microsoft.AspNetCore.Mvc;
using MarketSim.Core.Database;
using MarketSim.Core.Requests;
using MarketSim.Core.Services;

namespace MarketSim.Core.Controllers;

[ApiController]
[Route("[controller]")]
public class CashTransactionController : ControllerBase
{
    private readonly ILogger<CashTransactionController> _logger;
    private readonly AppDbContext _dbContext;
    private readonly ICashTransactionService _transactionService;

    public CashTransactionController(
        ILogger<CashTransactionController> logger,
        AppDbContext dbContext,
        ICashTransactionService transactionService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _transactionService = transactionService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction(CreateCashTransactionRequest request)
    {
        var transaction = await _transactionService.CreateCashTransaction(request);
        return Ok(transaction);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransaction(int id)
    {
        var transaction = await _dbContext.CashTransactions.FindAsync(id);
        return Ok(transaction);
    }
}
