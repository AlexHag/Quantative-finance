namespace Mld.Dataset.Elliptic;

public class EllipticDatasetOptions : IEquatable<EllipticDatasetOptions>
{
    public bool ReadAll = false;

    public bool ReadClasses = false;
    public int TxsClassesTake = 0;
    public string TxsClassesPath = "../elliptic_dataset/txs_classes.csv";

    public bool ReadEdgelist = false;
    public int TxsEdgelistTake = 0;
    public string TxsEdgelistPath = "../elliptic_dataset/txs_edgelist.csv";

    public bool ReadFeatures = false;
    public int TxsFeaturesTake = 0;
    public string TxsFeaturesPath = "../elliptic_dataset/small_txs_features.csv";

    public bool ReadDeanonymized = false;
    public int DeanonymizedTake = 0;
    public string DeanonymizedPath = "../elliptic_dataset/deanonymized.csv";

    public bool Equals(EllipticDatasetOptions? other)
    {
        if (other?.ReadAll != ReadAll)
            return false;

        if (other.ReadClasses != ReadClasses)
            return false;
        if (other?.TxsClassesTake != TxsClassesTake)
            return false;
        if (other?.TxsClassesPath != TxsClassesPath)
            return false;

        if (other?.ReadEdgelist != ReadEdgelist)
            return false;
        if (other?.TxsEdgelistTake != TxsEdgelistTake)
            return false;
        if (other?.TxsEdgelistPath != TxsEdgelistPath)
            return false;
        
        if (other?.ReadFeatures != ReadFeatures)
            return false;
        if (other?.TxsFeaturesPath != TxsFeaturesPath)
            return false;
        if (other?.TxsFeaturesPath != TxsFeaturesPath)
            return false;
        
        if (other?.ReadDeanonymized != ReadDeanonymized)
            return false;
        if (other?.DeanonymizedTake != DeanonymizedTake)
            return false;
        if (other?.DeanonymizedPath != DeanonymizedPath)
            return false;
        
        return true;
    }
}
