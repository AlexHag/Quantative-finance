using System.Text.Json;
using Mld.Dataset.IBM;

namespace Mld.Core;

public class Program
{
    public static void Main(string[] args)
    {
        var dataset = new IBMDataset();
        var datasetFrequency = new DatasetFrequency(dataset);

        // var currencyFrequency = datasetFrequency.CurrencyFrequency;
        var paymentFormatFrequency = datasetFrequency.PaymentFormatFrequency;

        // Console.WriteLine("Currency frequency:");
        // PrintObject(currencyFrequency);
        Console.WriteLine("\nPayment Format Frequency:");
        PrintObject(paymentFormatFrequency);
        // File.WriteAllText("KeyFigures/currencyFrequency.json", JsonSerialize(currencyFrequency));
        File.WriteAllText("KeyFigures/paymentFormatFrequency.json", JsonSerialize(paymentFormatFrequency));
    }

    public static string JsonSerialize(object obj)
        => JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });

    public static void PrintObject(object obj)
        => Console.WriteLine(JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true }));
}
