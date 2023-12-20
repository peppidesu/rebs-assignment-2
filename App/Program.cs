using System.Reflection;
using CommandLine;
using Core;
class Program
{
    public class Options
    {
        [Value(0, 
            MetaName = "log",
            HelpText = "The path to the event log file (.csv)",
            Required = true
        )]
        public required string Log { get; set; }
        [Value(1, 
            MetaName = "graph",
            HelpText = "The path to the DCR graph file (.yaml)",
            Required = true
        )]
        public required string Graph { get; set; }

        [Option("run")]
        public IEnumerable<string>? Runs { get; set; }        
    }

    static bool CheckValidPath(string path) {
        
        if (!File.Exists(path)) {
            Console.Error.WriteLine($"[Error] File does not exist: '{path}'");
            return false;
        }
        try {
            var fs = File.Open(path, FileMode.Open);
            fs.Dispose();
        }
        catch (UnauthorizedAccessException e) {
            Console.Error.WriteLine($"[Error] Cannot open file '{path}': Unauthorized access.");
            Console.Error.WriteLine($"Message: {e.Message}");
            return false;
        }
        catch (Exception e) {
            Console.Error.WriteLine($"[Error] Cannot open file '{path}': Unknown error.");
            Console.Error.WriteLine($"Message: {e.Message}");
            return false;
        }        
        
        return true;
    }
 
    static void Main(string[] args) {
        var parseResult = Parser.Default.ParseArguments<Options>(args);
        
        if (parseResult.Errors.Any()) {
            Environment.Exit(1001);
        }
        Options options = parseResult.Value;        

        if (!CheckValidPath(options.Log)) {
            Environment.Exit(1002);
        }
        
        if (!CheckValidPath(options.Graph)) {
            Environment.Exit(1003);            
        }
        
        var checker = new ConformanceChecker();
        var csvLoader = new CsvLoader();
        var yamlLoader = new YamlLoader();

        var logs = csvLoader.LoadCsv(options.Log);
        if (options.Runs.Any()) {
            // check for invalid run names
            if (options.Runs.Except(logs.Keys).Any()) {
                var invalid = options.Runs.Except(logs.Keys).First();
                Console.Error.WriteLine($"Invalid run '{invalid}'");
            }
            // filter selected runs only
            logs = logs.IntersectBy(options.Runs, pair => pair.Key).ToDictionary();
        }
        
        var graph = yamlLoader.LoadFromFile(options.Graph);
        
        var all = true;
        foreach (var pair in logs) {            
            var result = checker.IsConformant(ref graph, pair.Value);
            if (!result)
            {
                all = false;
                Console.WriteLine($"Run '{pair.Key}' is not conformant.");
            }
        }
        if(all) {
            Console.WriteLine("All runs are conformant.");
        }
    }
}


