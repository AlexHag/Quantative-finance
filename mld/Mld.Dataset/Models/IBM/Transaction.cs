namespace Mld.Dataset.IBM;

public class Transaction
{
    public string Id => $"{FromBank},{FromBankAccount},{ToBank},{ToBankAccount}";
    public DateTime Timestamp { get; set; }
    public string FromBank { get; set; }
    public string FromBankAccount { get; set; }
    public string ToBank { get; set; }
    public string ToBankAccount { get; set; }
    public double AmountPaid { get; set; }
    public double AmountReceived { get; set; }
    public string PaymentCurrency { get; set; }
    public string ReceivingCurrency { get; set; }
    public string PaymentFormat { get; set; }
    public bool IsLaundering { get; set; }
}

// Timestamp,From Bank,Account,To Bank,Account,Amount Received,Receiving Currency,Amount Paid,Payment Currency,Payment Format,Is Laundering
// 2022/09/01 00:20,010,8000EBD30,010,8000EBD30,3697.34,US Dollar,3697.34,US Dollar,Reinvestment,0
// 2022/09/01 00:20,03208,8000F4580,001,8000F5340,0.01,US Dollar,0.01,US Dollar,Cheque,0
// 2022/09/01 00:00,03209,8000F4670,03209,8000F4670,14675.57,US Dollar,14675.57,US Dollar,Reinvestment,0
// 2022/09/01 00:02,012,8000F5030,012,8000F5030,2806.97,US Dollar,2806.97,US Dollar,Reinvestment,0