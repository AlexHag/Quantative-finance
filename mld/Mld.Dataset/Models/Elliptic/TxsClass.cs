namespace Mld.Dataset.Elliptic;

public class TxsClass
{
    public int txId { get; set; }
    public TxsClassType type { get; set; }
}

public enum TxsClassType
{
    Illicit = 1,
    Licit = 2,
    Unknown = 3
}
