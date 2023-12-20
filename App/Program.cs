using System.Reflection;
using CommandLine;

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
        var result = Parser.Default.ParseArguments<Options>(args);
        
        if (result.Errors.Any()) {
            Environment.Exit(1001);
        }
        Options options = result.Value;        

        if (!CheckValidPath(options.Log)) {
            Environment.Exit(1002);
        }
        
        if (!CheckValidPath(options.Graph)) {
            Environment.Exit(1003);            
        }
        
        Console.WriteLine(options.Log);
        Console.WriteLine(options.Graph);
    }
}


