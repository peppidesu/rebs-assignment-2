namespace Core;

/// <summary>
/// Abstract class representing an event.
/// Events are only differentiated by their Id.
/// </summary>
public abstract class Event : IEquatable<Event> {
    public abstract string Id { get; }

    public override bool Equals(object? obj) => Equals(obj as Event);
    public bool Equals(Event? other)
    {
        return Id == other?.Id;
    }
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

}