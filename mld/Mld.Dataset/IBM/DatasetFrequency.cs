namespace Mld.Dataset.IBM;

public class DatasetFrequency
{
    private readonly IBMDataset _dataset;
    private readonly DatasetCategories _categories;

    public DatasetFrequency(IBMDataset dataset)
    {
        _dataset = dataset;
        _categories = new DatasetCategories(dataset);
    }

    public DatasetFrequency(DatasetCategories categories)
    {
        _categories = categories;
        _dataset = categories._dataset;
    }

    public Dictionary<string, int> CurrencyFrequency =>
        _receivingCurrencyFrequency ?? CalculateReceivingCurrencyFrequency(_categories.PaymentCurrencies);
    private Dictionary<string, int>? _receivingCurrencyFrequency;
    public Dictionary<string, int> CalculateReceivingCurrencyFrequency(IEnumerable<string> currencies)
    {
        if (_receivingCurrencyFrequency is not null)
            return _receivingCurrencyFrequency;

        _receivingCurrencyFrequency = new Dictionary<string, int>();
        foreach (var currency in currencies)
        {
            var receivingCurrencyCount = _dataset.Transactions
                .Where(p => p.ReceivingCurrency == currency)
                .Count();
            _receivingCurrencyFrequency.Add(currency, receivingCurrencyCount);
        }
        return _receivingCurrencyFrequency;
    }

    public Dictionary<string, int> PaymentFormatFrequency =>
        _paymentFormatFrequency ?? CalculatePaymentFormatFrequency(_categories.PaymentFormats);
    private Dictionary<string, int>? _paymentFormatFrequency;
    public Dictionary<string, int> CalculatePaymentFormatFrequency(IEnumerable<string> paymentFormats)
    {
        if (_paymentFormatFrequency is not null)
            return _paymentFormatFrequency;

        _paymentFormatFrequency = new Dictionary<string, int>();
        foreach (var paymentFormat in paymentFormats)
        {
            Console.WriteLine($"Counting: {paymentFormat}");
            var paymentFormatCount = _dataset.Transactions
                .Where(p => p.PaymentFormat == paymentFormat)
                .Count();
            Console.WriteLine($"Counted:  {paymentFormat} : {paymentFormatCount}");
            _paymentFormatFrequency.Add(paymentFormat, paymentFormatCount);
        }
        return _paymentFormatFrequency;
    }
}
