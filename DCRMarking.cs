namespace Core;

public struct DCRMarking<T> : ICloneable where T : notnull { 
    public readonly HashSet<T> executed = [], included = [], pending = [];

    public DCRMarking(HashSet<T> executed, HashSet<T> included, HashSet<T> pending) {
        this.executed = executed;
        this.included = included;
        this.pending = pending;
    }

    public readonly object Clone()
    {
        return new DCRMarking<T>(
            new HashSet<T>(executed), 
            new HashSet<T>(included), 
            new HashSet<T>(pending)
        );
    }
}