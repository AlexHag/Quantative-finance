
public class StockTransaction
{
    public Guid Id { get; set; }
    public Guid BuyOrderId { get; set; }
    public Guid SellOrderId { get; set; }
    public int StockId { get; set; }
    public double TradedAt { get; set; }
    public int Quantity { get; set; }
    public double OrderTotal => TradedAt * Quantity;
    public DateTime CreatedAt { get; set; }
}