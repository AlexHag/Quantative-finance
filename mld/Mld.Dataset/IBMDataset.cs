namespace Mld.Dataset.IBM;

public class IBMDataset
{
    private readonly IBMDatasetOptions _options;
    private List<Transaction>? _transactions;

    public IBMDataset(IBMDatasetOptions options)
    {
        _options = options;
        if (_options.ReadTransactions)
            ReadDataset(_options.Path, _options.Take);
    }

    public IBMDataset()
    {
        _options = new IBMDatasetOptions();
    }

    public List<Transaction> Transactions =>
        _transactions ?? ReadDataset(_options.Path, _options.Take);

    private List<Transaction> ReadDataset(string path, int take = 0)
    {
        if (_transactions is not null)
            return _transactions;
        _transactions = new List<Transaction>();

        Console.WriteLine("DEBUG: Reading IBM transactions");
        var filecontent = File.ReadAllLines(path);

        take = take == 0 ? filecontent.Count() : take;
        for (int i = 1; i < take; i++)
        {
            var row = filecontent[i].Split(",");
            var transaction = new Transaction
            {
                Timestamp = DateTime.Parse(row[0]),
                FromBank = row[1],
                FromBankAccount = row[2],
                ToBank = row[3],
                ToBankAccount = row[4],
                AmountReceived = double.Parse(row[5]),
                ReceivingCurrency = row[6],
                AmountPaid = double.Parse(row[7]),
                PaymentCurrency = row[8],
                PaymentFormat = row[9],
                IsLaundering = Convert.ToBoolean(int.Parse(row[10]))
            };
            _transactions.Add(transaction);
        }

        return _transactions;
    }
}