using MarketSim.Core.Entities;
using MarketSim.Core.Database;
using MarketSim.Core.Requests;

namespace MarketSim.Core.Services;

public interface ICashTransactionService
{
    Task<CashTransaction> CreateCashTransaction(CreateCashTransactionRequest request);
}

public class CashTransactionService : ICashTransactionService
{
    private readonly AppDbContext _dbContext;
    private readonly SystemSettings _system;

    public CashTransactionService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _system = _dbContext.SystemSettings.FirstOrDefault()!;
    }

    public async Task<CashTransaction> CreateCashTransaction(CreateCashTransactionRequest request)
    {
        var portfolio = await _dbContext.Portfolios.FindAsync(request.PortfolioId);
        if (portfolio is null)
            throw new Exception("Portfolio not found");

        var transaction = new CashTransaction
        {
            Portfolio = portfolio,
            Amount = request.Amount,
            Type = request.Type,
            CreatedAt = _system.CurrentDay
        };

        if (transaction.Type == CashTransactionType.Deposit)
        {
            portfolio.CashBalance += transaction.Amount;
        }
        else
        {
            if (portfolio.CashBalance < transaction.Amount)
                throw new Exception("Insufficient balance");
            
            portfolio.CashBalance -= transaction.Amount;
        }

        portfolio.CashTransactions.Add(transaction);
        await _dbContext.SaveChangesAsync();

        return transaction;
    }
}