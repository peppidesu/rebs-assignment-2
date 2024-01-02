using System.Diagnostics;

namespace Core;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public class LogEvent : Event {
    public readonly string Name;    
    public readonly string Run, Date;    
    public override string Id => Name;

    public LogEvent(string name, string run, string date) {
        Name = name;
        Run = run;
        Date = date;
    }

    public override string ToString() => Name;

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}