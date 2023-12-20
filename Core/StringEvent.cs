namespace Core;


public struct StringEvent : IEvent {
    public readonly string Name;

    public StringEvent(string name) {
        Name = name;
    }
}