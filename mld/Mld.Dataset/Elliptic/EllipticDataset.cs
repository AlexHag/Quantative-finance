namespace Mld.Dataset.Elliptic;

public class EllipticDataset
{
    private readonly EllipticDatasetOptions _options;
    private List<TxsClass>? _txsClasses;
    private List<TxsEdge>? _txsEdges;
    private List<TxsFeature>? _txsFeatures;
    private List<Deanonymized>? _deanonymized;

    public EllipticDataset(EllipticDatasetOptions options)
    {
        _options = options;

        if (options.ReadAll)
        {
            _txsClasses = ReadClasses(options.TxsClassesPath, options.TxsClassesTake);
            _txsEdges = ReadEdgelist(options.TxsEdgelistPath, options.TxsEdgelistTake);
            _txsFeatures = ReadFeatures(options.TxsFeaturesPath, options.TxsFeaturesTake);
            _deanonymized = ReadDeanonymized(options.DeanonymizedPath, options.DeanonymizedTake);
        }
        else if (options.ReadClasses)
            _txsClasses = ReadClasses(options.TxsClassesPath, options.TxsClassesTake);
        
        else if (options.ReadEdgelist)
            _txsEdges = ReadEdgelist(options.TxsEdgelistPath, options.TxsEdgelistTake);
        
        else if (options.ReadFeatures)
            _txsFeatures = ReadFeatures(options.TxsFeaturesPath, options.TxsFeaturesTake);
        
        else if (options.ReadDeanonymized)
            _deanonymized = ReadDeanonymized(options.DeanonymizedPath, options.DeanonymizedTake);
    }

    public EllipticDataset()
    {
        _options = new EllipticDatasetOptions();
    }

    public List<TxsClass> TsxClasses => 
        _txsClasses ?? ReadClasses(_options.TxsClassesPath, _options.TxsClassesTake);

    public List<TxsEdge> TsxEdges => 
        _txsEdges ?? ReadEdgelist(_options.TxsEdgelistPath, _options.TxsEdgelistTake);

    public List<TxsFeature> TsxFeatures => 
        _txsFeatures ?? ReadFeatures(_options.TxsFeaturesPath, _options.TxsFeaturesTake);
    
    public List<Deanonymized> Deanonymized =>
        _deanonymized ?? ReadDeanonymized(_options.DeanonymizedPath, _options.DeanonymizedTake);

    private List<TxsClass> ReadClasses(string path, int take = 0)
    {
        if (_txsClasses is not null)
            return _txsClasses;
        _txsClasses = new List<TxsClass>();

        Console.WriteLine("DEBUG: Reading classes...");
        var filecontent = File.ReadAllLines(path);

        take = take == 0 ? filecontent.Count() : take;
        for(int i = 1; i < take; i++)
        {
            var row = filecontent[i].Split(",");
            var tsxClass = new TxsClass
            {
                txId = int.Parse(row[0]),
                type = Enum.Parse<TxsClassType>(row[1])
            };
            _txsClasses.Add(tsxClass);
        }
        
        _txsClasses = _txsClasses.OrderBy(p => p.txId).ToList();
        return _txsClasses;
    }

    private List<TxsEdge> ReadEdgelist(string path, int take = 0)
    {
        if (_txsEdges is not null)
            return _txsEdges;
        _txsEdges = new List<TxsEdge>();

        Console.WriteLine("DEBUG: Reading edges...");
        var filecontent = File.ReadAllLines(path);

        take = take == 0 ? filecontent.Count() : take;
        for(int i = 1; i < take; i++)
        {
            var row = filecontent[i].Split(",");
            var tsxEdge = new TxsEdge
            {
                txId1 = int.Parse(row[0]),
                txId2 = int.Parse(row[1])
            };
            _txsEdges.Add(tsxEdge);
        }

        _txsEdges = _txsEdges.OrderBy(p => p.txId1).ToList();
        return _txsEdges;
    }

    private List<TxsFeature> ReadFeatures(string path, int take = 0)
    {
        if (_txsFeatures is not null)
            return _txsFeatures;
        _txsFeatures = new List<TxsFeature>();

        Console.WriteLine("DEBUG: Reading features...");
        var filecontent = File.ReadAllLines(path);

        take = take == 0 ? filecontent.Count() : take;
        for(int i = 1; i < take; i++)
        {
            var row = filecontent[i].Split(",");
            try
            {
                var tsxFeature = new TxsFeature
                {
                    txId = int.Parse(row[0]),
                    time_step = int.Parse(row[1]),
                    in_txs_degree = double.Parse(row[2]),
                    out_txs_degree = double.Parse(row[3]),
                    total_BTC = double.Parse(row[4]),
                    fees = double.Parse(row[5]),
                    size = double.Parse(row[6]),
                    num_input_addresses = double.Parse(row[7]),
                    num_output_addresses = double.Parse(row[8]),
                    in_BTC_min = double.Parse(row[9]),
                    in_BTC_max = double.Parse(row[10]),
                    in_BTC_mean = double.Parse(row[11]),
                    in_BTC_median = double.Parse(row[12]),
                    in_BTC_total = double.Parse(row[13]),
                    out_BTC_min = double.Parse(row[14]),
                    out_BTC_max = double.Parse(row[15]),
                    out_BTC_mean = double.Parse(row[16]),
                    out_BTC_median = double.Parse(row[17]),
                    out_BTC_total = double.Parse(row[18]),
                };
                _txsFeatures.Add(tsxFeature);
            }
            catch 
            {
                continue;
            }
        }
        _txsFeatures = _txsFeatures.OrderBy(p => p.txId).ToList();
        return _txsFeatures;
    }

    private List<Deanonymized> ReadDeanonymized(string path, int take = 0)
    {
        if (_deanonymized is not null)
            return _deanonymized;
        _deanonymized = new List<Deanonymized>();

        Console.WriteLine("DEBUG: Reading deanonymized...");
        var filecontent = File.ReadAllLines(path);

        take = take == 0 ? filecontent.Count() : take;
        for(int i = 1; i < take; i++)
        {
            var row = filecontent[i].Split(",");
            var deanonymize = new Deanonymized
            {
                txId = int.Parse(row[0]),
                transaction = row[1]
            };
            _deanonymized.Add(deanonymize);
        }

        _deanonymized = _deanonymized.OrderBy(p => p.txId).ToList();
        return _deanonymized;
    }
}
