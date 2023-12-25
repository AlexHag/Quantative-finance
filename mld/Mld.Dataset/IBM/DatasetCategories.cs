using System.Text.Json;

namespace Mld.Dataset.IBM;

public class DatasetCategories
{
    public IBMDataset _dataset;
    public DatasetCategories(IBMDataset dataset)
    {
        _dataset = dataset;
    }

    public IEnumerable<string> PaymentCurrencies
        => _paymentCurrencies ?? CalculatePaymentCurrencies();
    private IEnumerable<string>? _paymentCurrencies;
    private IEnumerable<string> CalculatePaymentCurrencies() 
    {
        if (_paymentCurrencies is not null)
            return _paymentCurrencies;

        Console.WriteLine("DEBUG: Calculating distinct payment currencies...");

        _paymentCurrencies = _dataset.Transactions
            .DistinctBy(p => p.PaymentCurrency)
            .Select(p => p.PaymentCurrency);
        
        return _paymentCurrencies;
    }

    public IEnumerable<string> ReceivingCurrencies
        => _receivingCurrencies ?? CalculateReceivingCurrencies();
    private IEnumerable<string>? _receivingCurrencies;
    private IEnumerable<string> CalculateReceivingCurrencies() 
    {
        if (_receivingCurrencies is not null)
            return _receivingCurrencies;

        Console.WriteLine("DEBUG: Calculating distinct receiving currencies...");

        _receivingCurrencies = _dataset.Transactions
            .DistinctBy(p => p.ReceivingCurrency)
            .Select(p => p.ReceivingCurrency);
        
        return _receivingCurrencies;
    }

    public IEnumerable<string> FromBanks =>
        _fromBanks ?? CalculateFromBanks();
    private IEnumerable<string>? _fromBanks;
    private IEnumerable<string> CalculateFromBanks()
    {
        if (_fromBanks is not null)
            return _fromBanks;
        
        Console.WriteLine("DEBUG: Calculating distinct from banks");

        _fromBanks = _dataset.Transactions
            .DistinctBy(p => p.FromBank)
            .Select(p => p.FromBank);
        
        return _fromBanks;
    }

    public IEnumerable<string> ToBanks =>
        _toBanks ?? CalculateToBanks();
    private IEnumerable<string>? _toBanks;
    private IEnumerable<string> CalculateToBanks()
    {
        if (_toBanks is not null)
            return _toBanks;
        
        Console.WriteLine("DEBUG: Calculating distinct to banks");

        _toBanks = _dataset.Transactions
            .DistinctBy(p => p.FromBank)
            .Select(p => p.FromBank);
        
        return _toBanks;
    }

    public IEnumerable<string> PaymentFormats =>
        _paymentFormats ?? CalculatePaymentFormats();
    private IEnumerable<string>? _paymentFormats;
    private IEnumerable<string> CalculatePaymentFormats()
    {
        if (_paymentFormats is not null)
            return _paymentFormats;
        
        Console.WriteLine("DEBUG: Calculating distinct payment formats");

        _paymentFormats = _dataset.Transactions
            .DistinctBy(p => p.PaymentFormat)
            .Select(p => p.PaymentFormat);
        
        return _paymentFormats;
    }

    public static void PrintObject(object obj)
        => Console.WriteLine(JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true }));
}
