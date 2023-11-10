public class StockService
{
    public AppDbContext _dbContext;

    public StockService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void HandleBuyOrder(StockBuyOrder buyOrder)
    {
        _dbContext.StockBuyOrders.Add(buyOrder);

        var sellOrders = _dbContext.StockSellOrders.Where(p => 
            p.StockId == buyOrder.StockId &&
            p.SellPrice <= buyOrder.BuyPrice &&
            (p.Status == StockOrderStatus.Pending ||
            p.Status == StockOrderStatus.PartiallyFulfilled))
            .OrderBy(p => p.SellPrice);
        
        if (sellOrders is null)
        {
            Console.WriteLine("No sell orders for this buy order");
            return;
        }

        foreach (var sellOrder in sellOrders)
        {
            if (buyOrder.QuantityFilled < buyOrder.Quantity)
            {
                // If the amount bought plus what the seller wants to sell is more than what the buyer wants to buy, we buy parts of the sell order
                if (buyOrder.QuantityFilled + sellOrder.Quantity > buyOrder.Quantity)
                {
                    // Partially fill sell order = completely fill buy order
                    var quantityLeft = buyOrder.Quantity - buyOrder.QuantityFilled;
                    CreateTransaction(sellOrder, buyOrder, quantityLeft);
                    break;
                }

                CreateTransaction(sellOrder, buyOrder, sellOrder.Quantity);

                if (buyOrder.QuantityFilled == buyOrder.Quantity)
                {
                    break;
                }
            }
        }
    }

    public void HandleSellOrder(StockSellOrder sellOrder)
    {
        var position = _dbContext.StockPositions.SingleOrDefault(p =>
            p.UserId == sellOrder.UserId &&
            p.StockId == sellOrder.StockId);
        if (position is null)
        {
            throw new Exception("Cant place sell order, no position in this stock");
        }
        if (position.Quantity < sellOrder.Quantity)
        {
            throw new Exception("Cant place sell order, not enought stocks");
        }

        _dbContext.StockSellOrders.Add(sellOrder);

        var buyOrders = _dbContext.StockBuyOrders.Where(p => 
            p.StockId == sellOrder.StockId &&
            p.BuyPrice >= sellOrder.SellPrice &&
            (p.Status == StockOrderStatus.Pending ||
            p.Status == StockOrderStatus.PartiallyFulfilled))
            .OrderByDescending(p => p.BuyPrice)
            .ToList();
        
        if (buyOrders is null)
        {
            Console.WriteLine("No buy orders for sell order");
            return;
        }

        // var quantityFilled = 0;
        foreach (var buyOrder in buyOrders)
        {
            if (sellOrder.QuantityFilled < sellOrder.Quantity)
            {
                if (sellOrder.QuantityFilled + buyOrder.Quantity > sellOrder.Quantity)
                {
                    // Partial fill buy order = completely fill sell order
                    Console.WriteLine("Partially filling buy order");
                    var quantityLeft = sellOrder.Quantity - sellOrder.QuantityFilled;
                    CreateTransaction(sellOrder, buyOrder, quantityLeft);
                    break;
                }
                Console.WriteLine("Filling buy order");
                CreateTransaction(sellOrder, buyOrder, buyOrder.Quantity);
                if (sellOrder.QuantityFilled == sellOrder.Quantity)
                {
                    break;
                }
            }
        }
    }

    public void CreateTransaction(StockSellOrder sellOrder, StockBuyOrder buyOrder, int quantityToFill)
    {
        if (buyOrder.BuyPrice < sellOrder.SellPrice)
        {
            throw new Exception($"Cannot create transaction. Buy price {buyOrder.BuyPrice}$ is less than sell price {sellOrder.SellPrice}$");
        }

        if (sellOrder.StockId != buyOrder.StockId)
        {
            throw new Exception($"Cannot create transaction. Sell and buy order are for different stocks. SellOrder.StockId = {sellOrder.StockId}. BuyOrder.StockId = {buyOrder.StockId}");
        }

        var sellerPosition = _dbContext.StockPositions.SingleOrDefault(p => 
            p.UserId == sellOrder.UserId &&
            p.StockId == sellOrder.StockId);

        if (sellerPosition is null)
        {
            throw new Exception("Seller does not have any stocks to sell");
        }
        
        if (sellerPosition.Quantity < quantityToFill)
        {
            throw new Exception($"Selled does not have enought stocks to sell. Seller quantity {sellerPosition.Quantity}, Quantity to fill: {quantityToFill}");
        }

        var sellOrderQuantityLeft = sellOrder.Quantity - sellOrder.QuantityFilled;
        if (sellOrderQuantityLeft < quantityToFill)
        {
            throw new Exception("Cannot create transaction, sell order quantity left is less than quantity to fill");
        }

        var buyOrderQuantityLeft = buyOrder.Quantity - buyOrder.QuantityFilled;
        if (buyOrderQuantityLeft < quantityToFill)
        {
            throw new Exception("Cannot create transaction, buy order quantity left is less than quantity to fill");
        }

        buyOrder.QuantityFilled += quantityToFill;
        if (buyOrder.QuantityFilled == buyOrder.Quantity)
        {
            buyOrder.Status = StockOrderStatus.Fulfilled;
            buyOrder.FulfilledAt = DateTime.UtcNow;
        }
        else
        {
            buyOrder.Status = StockOrderStatus.PartiallyFulfilled;
        }

        sellerPosition.Quantity -= quantityToFill;
        sellOrder.QuantityFilled += quantityToFill;
        if (sellOrder.QuantityFilled == sellOrder.Quantity)
        {
            sellOrder.Status = StockOrderStatus.Fulfilled;
            sellOrder.FulfilledAt = DateTime.UtcNow;
        }
        else
        {
            sellOrder.Status = StockOrderStatus.PartiallyFulfilled;
        }

        var transaction = new StockTransaction
        {
            Id = Guid.NewGuid(),
            BuyOrderId = buyOrder.Id,
            SellOrderId = sellOrder.Id,
            StockId = sellOrder.StockId,
            TradedAt = sellOrder.SellPrice,
            Quantity = quantityToFill,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.StockTransactions.Add(transaction);

        var buyerPosition = _dbContext.StockPositions.SingleOrDefault(p => 
            p.UserId == buyOrder.UserId &&
            p.StockId == buyOrder.StockId);
        
        if (buyerPosition is null)
        {
            var newPosition = new StockPosition
            {
                Id = Guid.NewGuid(),
                StockId = buyOrder.StockId,
                UserId = buyOrder.UserId,
                Quantity = quantityToFill
            };
            _dbContext.StockPositions.Add(newPosition);
        }
        else
        {
            buyerPosition.Quantity += quantityToFill;
        }
    }
}