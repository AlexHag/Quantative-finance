using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MarketSim.Core.Database;
using MarketSim.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICashTransactionService, CashTransactionService>();
builder.Services.AddScoped<IStockTransactionService, StockTransactionService>();

builder.Services.AddControllers().AddJsonOptions(x =>
    {
        // x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        // x.JsonSerializerOptions.Refe
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
