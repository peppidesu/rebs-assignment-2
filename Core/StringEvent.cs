using System.Diagnostics;

namespace Core;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public class StringEvent : Event {
    public readonly string Name;    
    public override string Id => Name;

    public StringEvent(string name) {
        Name = name;
    }

    public override string ToString() => Name;

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}