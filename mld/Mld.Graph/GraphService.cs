using System.Text.Json;
using System.Text.Json.Serialization;
using Mld.Domain;
using Mld.Dataset.IBM;
using Neo4j.Driver;

namespace Mld.Graph;

public class GraphService
{
    private readonly IAsyncSession _session;
    public GraphService(string uri, string user, string password)
    {
        _session = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password)).AsyncSession();
    }

    public async Task UploadFile()
    {
        var query = @"LOAD CSV WITH HEADERS FROM 'file:///HI-Small_Trans.csv' AS row
                    MERGE (from:Account { accountNumber: row.`From Account`, bankNumber: row.`From Bank` })
                    MERGE (to:Account { accountNumber: row.`To Account`, bankNumber: row.`To Bank` })
                    CREATE (from)-[:Transaction {
                        timestamp: row.Timestamp,
                        amountPaid: toFloat(row.`Amount Paid`),
                        amountReceived: toFloat(row.`Amount Received`),
                        paymentCurrency: row.`Payment Currency`,
                        receivingCurrency: row.`Receiving Currency`,
                        format: row.`Payment Format`,
                        isLaundering: toBoolean(row.`Is Laundering`)
                    }]->(to)";
        await _session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync(query);
        });
    }

    public async Task CreateTransaction(Dataset.IBM.Transaction transaction)
    {
        var query = @"MERGE (from:Account
                    {
                        accountNumber: $FromBankAccount,
                        bankNumber: $FromBank
                    })
                    MERGE (to:Account
                    {
                        accountNumber: $ToBankAccount,
                        bankNumber: $ToBank
                    })
                    CREATE (from)-[:Transaction
                    {
                        timestamp: $Timestamp,
                        amountPaid: $AmountPaid,
                        amountReceived: $AmountReceived,
                        paymentCurrency: $PaymentCurrency,
                        receivingCurrency: $ReceivingCurrency,
                        format: $PaymentFormat,
                        isLaundering: $IsLaundering
                    }]->(to)";
        await _session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync(query, transaction);
        });
    }

    public async Task CreateAccount(string accountNumber, string bankNumber)
    {
        var query = "MERGE (:Account {accountNumber: $accountNumber, bankNumber: $bankNumber})";
        var param = new { accountNumber, bankNumber };

        await _session.ExecuteWriteAsync(async tx =>
        {
            var result = await tx.RunAsync(query, param);
        });
    }

    // public async Task<Account> GetAccount(string accountNumber)
    // {
    //     var query = "MATCH (n:Account {accountNumber: $accountNumber}) return n";
    //     var parameters = new { accountNumber };

    //     var result = await _session.ExecuteReadAsync(async tx =>
    //     {
    //         var res = await tx.RunAsync(query, parameters);
    //         var record = await res.SingleAsync();
    //         var nodeProps = JsonSerializer.Serialize(record[0].As<INode>().Properties);
    //         Console.WriteLine(nodeProps);
    //         return JsonSerializer.Deserialize<Account>(nodeProps);
    //     });

    //     return result;
    // }

    public static void PrintObject(object obj) =>
        Console.WriteLine(JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true }));

    public async Task SeedGraph()
    {
        var transactions = new List<Dataset.IBM.Transaction>()
        {
            new Dataset.IBM.Transaction
            {
                Timestamp = new DateTime(2022, 09, 01),
                FromBank = "01",
                FromBankAccount = "0001",
                ToBank = "01",
                ToBankAccount = "0002",
                AmountReceived = 20.0,
                ReceivingCurrency = "SEK",
                AmountPaid = 20.0,
                PaymentCurrency = "SEK",
                PaymentFormat = "Wire"
            },
            new Dataset.IBM.Transaction
            {
                Timestamp = new DateTime(2022, 09, 01),
                FromBank = "01",
                FromBankAccount = "0002",
                ToBank = "02",
                ToBankAccount = "0003",
                AmountReceived = 15.0,
                ReceivingCurrency = "SEK",
                AmountPaid = 15.05,
                PaymentCurrency = "SEK",
                PaymentFormat = "Wire"
            },
            new Dataset.IBM.Transaction
            {
                Timestamp = new DateTime(2022, 09, 01),
                FromBank = "01",
                FromBankAccount = "0001",
                ToBank = "02",
                ToBankAccount = "0003",
                AmountReceived = 20.0,
                ReceivingCurrency = "SEK",
                AmountPaid = 20.0,
                PaymentCurrency = "SEK",
                PaymentFormat = "Wire"
            },
        };

        foreach (var transaction in transactions)
        {
            await CreateTransaction(transaction);
        }
    }
}