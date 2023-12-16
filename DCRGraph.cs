namespace Core;

/// <summary>
/// Implementation of a DCR Graph.
/// </summary>
/// <typeparam name="T"></typeparam>
public class DCRGraph<T> where T : notnull { 

    // nodes
    private readonly HashSet<T> _events = []; 

    // edges
    private readonly Dictionary<T, HashSet<T>> _conditions = [],
                                               _milestones = [],
                                               _responses = [],
                                               _excludes = [],
                                               _includes = [];
    
    private DCRMarking<T> _marking = new DCRMarking<T>([], [], []);

    // simple constructor
    public DCRGraph(HashSet<T> events, 
                    Dictionary<T, HashSet<T>> conditions, 
                    Dictionary<T, HashSet<T>> milestones, 
                    Dictionary<T, HashSet<T>> responses, 
                    Dictionary<T, HashSet<T>> excludes, 
                    Dictionary<T, HashSet<T>> includes) {
        _events = events;
        _conditions = conditions;
        _milestones = milestones;
        _responses = responses;
        _excludes = excludes;
        _includes = includes;
    }

    // return deep copy to avoid mutation
    public DCRMarking<T> Marking() {        
        return (DCRMarking<T>) _marking.Clone(); 
    }
} 
