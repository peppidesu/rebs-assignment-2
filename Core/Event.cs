namespace Core;

public abstract class Event : IEquatable<Event> {
    public abstract string Id { get; }

    public bool Equals(Event? other)
    {
        return Id == other?.Id;
    }
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}