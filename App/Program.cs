using System.Reflection;
using CommandLine;

class Program
{
    public class Options
    {
        [Value(0, Required = true, HelpText = "The path to the ")]
        public required string Log { get; set; }
        [Value(1, Required = true)]
        public required string Graph { get; set; }
    }

    static string? OpenFileSafe(string path) {
        
        try {
            string result = File.ReadAllText(path);
            Console.WriteLine();
            return result;
        }
        catch (DirectoryNotFoundException) {
            Console.Error.WriteLine($"[Error] Path does not exist: '{path}'");
            
        }
        catch (FileNotFoundException) {
            Console.Error.WriteLine($"[Error] Path does not exist: '{path}'");
        }
        catch (UnauthorizedAccessException e) {
            Console.Error.WriteLine($"[Error] Cannot open file '{path}': Unauthorized access.");
            Console.Error.WriteLine($"Message: {e.Message}");
        }
        catch (Exception e) {
            Console.Error.WriteLine($"[Error] Cannot open file '{path}': Unknown error.");
            Console.Error.WriteLine($"Message: {e.Message}");
        }        
        return null;
    }
 
    static void Main(string[] args) {
        var result = Parser.Default.ParseArguments<Options>(args);
        
        if (result.Errors.Any()) {
            Environment.Exit(1001);
        }
        Options options = result.Value;        

        string? log = OpenFileSafe(options.Log);
        if (log == null) {
            Environment.Exit(1002);
        }
        string? graph = OpenFileSafe(options.Graph);
        if (graph == null) {
            Environment.Exit(1003);            
        }
        
        Console.WriteLine(log);
        Console.WriteLine(graph);
    }
}


