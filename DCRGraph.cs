namespace Core;

/// <summary>
/// Implementation of a DCR Graph.
/// </summary>
/// <typeparam name="T"></typeparam>
public class DCRGraph<T> where T : notnull { 
    private HashSet<T> _events = []; 
    private Dictionary<T, HashSet<T>> _conditions = []; 
    private Dictionary<T, HashSet<T>> _milestones = []; 
    private Dictionary<T, HashSet<T>> _responses = []; 
    private Dictionary<T, HashSet<T>> _excludes = []; 
    private Dictionary<T, HashSet<T>> _includes = [];
    
    private DCRMarking<T> _marking = new DCRMarking<T>([], [], []);

    public DCRGraph(HashSet<T> events, Dictionary<T, HashSet<T>> conditions, Dictionary<T, HashSet<T>> milestones, Dictionary<T, HashSet<T>> responses, Dictionary<T, HashSet<T>> excludes, Dictionary<T, HashSet<T>> includes) {
        _events = events;
        _conditions = conditions;
        _milestones = milestones;
        _responses = responses;
        _excludes = excludes;
        _includes = includes;
    }

    public DCRMarking<T> Marking() {        
        return (DCRMarking<T>) _marking.Clone(); // deep copy
    }
} 
