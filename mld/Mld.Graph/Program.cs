using System.Text.Json;
using Mld.Dataset.IBM;
using Mld.Bogus;
using Neo4j.Driver;

namespace Mld.Graph;

public class Program
{
    public static async Task Main(string[] args)
    {
        var graph = new MyGraphService("bolt://localhost:7687", "neo4j", "p@ssword1!");

        var peopleGenerator = new PeopleGenerator();
        string[] companies = { "Apple", "Salt", "Unemployed", "School" };
        var random = new Random();
        
        for (int i = 0; i < 20; i++)
        {
            var person = peopleGenerator.GenerateRandomPerson();
            await graph.CreatePerson(person);
            await graph.CreateEmployment(person, companies[random.Next(0, companies.Length)], random.Next(0, 100000));
        }

        
        
        
        // var dataset = new IBMDataset();
        // await graph.UploadFile();

        // var nonSelfTransactions = dataset.Transactions.Take(50000); //.Where(p => p.FromBankAccount != p.ToBankAccount).Take(500);
        // int amountOfLaunderings = 0;
        // foreach(var transaction in nonSelfTransactions)
        // {
        //     if (transaction.IsLaundering)
        //         amountOfLaunderings++;
        //     await graph.CreateTransaction(transaction);            
        // }
        // Console.WriteLine(amountOfLaunderings);
        // await graph.SeedGraph();
        // await graph.CreateAccount("0001", "01");
        // var account = await graph.GetAccount("0001");
        // PrintObject(account);
        // await graph.FindAccount("0001");
    }
    public static void PrintObject(object obj) =>
        Console.WriteLine(JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true }));
}

public class GraphExample : IDisposable
{
    private readonly IDriver _driver;
    public GraphExample(string uri, string user, string password)
    {
        _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
    }

    public async Task CreateTransaction()
    {
        using var session = _driver.AsyncSession();
        var createUserQuery = "CREATE (:User {name: $name, age: $age})";
        var user1Param = new { name = "Alex", age=19 };
        var user2Param = new { name = "Paul", age=25 };

        var user1write = await session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync(createUserQuery, user1Param);
            return "wrote user 1...";
        });
        var user2write = await session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync(createUserQuery, user2Param);
            return "wrote user 1...";
        });

        Console.WriteLine($"User1write: {user1write}");
        Console.WriteLine($"User2write: {user2write}");

        var createTransactionQuery = "MATCH (sender:User {name: $sender}), (receiver:User {name: $receiver}) " +
                                     "CREATE (sender)-[:TRANSFER {amount: $amount}]->(receiver)";
        var transactionParam = new { sender = "Alex", receiver = "Paul", amount = 100.00 };

        var transactionWrite = await session.ExecuteWriteAsync(async tx =>
        {
            await tx.RunAsync(createTransactionQuery, transactionParam);
            return "wrote transaction...";
        });

        Console.WriteLine($"TransactionWrite: {transactionWrite}");
    }

    public void Dispose()
    {
        _driver?.Dispose();
    }
}
