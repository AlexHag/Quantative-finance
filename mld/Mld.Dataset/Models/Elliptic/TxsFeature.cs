namespace Mld.Dataset.Elliptic;

public class TxsFeature
{
    public int txId { get; set; }
    public int time_step { get; set; }
    public double in_txs_degree { get; set; }
    public double out_txs_degree { get; set; }
    public double total_BTC { get; set; }
    public double fees { get; set; }
    public double size { get; set; }
    public double num_input_addresses { get; set; }
    public double num_output_addresses { get; set; }
    public double in_BTC_min { get; set; }
    public double in_BTC_max { get; set; }
    public double in_BTC_mean { get; set; }
    public double in_BTC_median { get; set; }
    public double in_BTC_total { get; set; }
    public double out_BTC_min { get; set; }
    public double out_BTC_max { get; set; }
    public double out_BTC_mean { get; set; }
    public double out_BTC_median { get; set; }
    public double out_BTC_total { get; set; }
}
