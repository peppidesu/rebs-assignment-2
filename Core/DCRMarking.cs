namespace Core;

public struct DCRMarking { 
    public readonly HashSet<Event> Executed = [], Included = [], Pending = [];

    public DCRMarking(HashSet<Event> executed, HashSet<Event> included, HashSet<Event> pending) {
        Executed = executed;
        Included = included;
        Pending = pending;
    }

    public readonly object Clone()
    {
        return new DCRMarking(
            new HashSet<Event>(Executed), 
            new HashSet<Event>(Included), 
            new HashSet<Event>(Pending)
        );
    }
}