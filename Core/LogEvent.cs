using System.Diagnostics;

namespace Core;
/// <summary>
/// Represents an event in the event log.
/// Contains additional Run and Date fields.
/// /// </summary>
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

    public override string ToString() => $"{Name} (run {Run}, {Date})";

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}