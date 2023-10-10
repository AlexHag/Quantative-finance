using MarketSim.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MarketSim.Core.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    { }

    public DbSet<SystemSettings> SystemSettings { get; set; }
    public DbSet<Stock> Stocks { get; set; }

    public DbSet<Portfolio> Portfolios { get; set; }

    public DbSet<CashTransaction> CashTransactions { get; set; }
    public DbSet<StockTransaction> StockTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   
        modelBuilder.Entity<CashTransaction>()
            .Property(p => p.Type)
            .HasConversion(new EnumToStringConverter<CashTransactionType>());

        modelBuilder.Entity<StockTransaction>()
            .Property(p => p.Type)
            .HasConversion(new EnumToStringConverter<StockTransactionType>());
        
        modelBuilder.Entity<Stock>()
            .HasAlternateKey(p => p.Ticker);
    }
}
