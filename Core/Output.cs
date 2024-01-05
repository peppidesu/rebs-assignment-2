namespace Core;

/// <summary>
/// Log level of the logger
/// </summary>
public enum LogLevel {
    Trace,
    Info,
    Warning,
    Error
}

/// <summary>
/// Simple logging class
/// </summary>
public class Output {
    public static LogLevel Level { get; set; } = LogLevel.Info;
    
    public static void Trace(string text) {
        if (Level == LogLevel.Trace) {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"[TRACE] {text}");
            Console.ResetColor();
        }        
    }
    public static void Info(string text) {
        if (Level <= LogLevel.Info) {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[INFO] {text}");
            Console.ResetColor();
        }        
    }
    public static void Warning(string text) {
        if (Level <= LogLevel.Warning) {
            Console.ForegroundColor = ConsoleColor.Yellow;            
            Console.WriteLine($"[WARN] {text}");
            Console.ResetColor();
        }   
    }
    public static void Error(string text) {
        Console.ForegroundColor = ConsoleColor.Red;            
        Console.WriteLine($"[ERROR] {text}");
        Console.ResetColor();
    }
}