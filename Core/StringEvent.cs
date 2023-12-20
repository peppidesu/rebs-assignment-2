using System.Diagnostics;

namespace Core;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public struct StringEvent : IEvent {
    public readonly string Name;

    public StringEvent(string name) {
        Name = name;
    }

    public override readonly string ToString() => Name;

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}