namespace MarketSim.Core.Entities;

public class CashTransaction
{
    public int Id { get; set; }
    public required Portfolio Portfolio { get; set; }
    public double Amount { get; set; }
    public CashTransactionType Type { get; set; }
    public DateTime CreatedAt { get; set; }
}

public enum CashTransactionType
{
    Deposit = 1,
    Withdrawal = 2
}
