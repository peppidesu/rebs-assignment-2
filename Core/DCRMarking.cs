namespace Core;

public struct DCRMarking<T> : ICloneable where T : IEvent { 
    public readonly HashSet<T> Executed = [], Included = [], Pending = [];

    public DCRMarking(HashSet<T> executed, HashSet<T> included, HashSet<T> pending) {
        Executed = executed;
        Included = included;
        Pending = pending;
    }

    public readonly object Clone()
    {
        return new DCRMarking<T>(
            new HashSet<T>(Executed), 
            new HashSet<T>(Included), 
            new HashSet<T>(Pending)
        );
    }
}