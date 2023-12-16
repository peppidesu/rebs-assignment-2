namespace Core;

/// <summary>
/// Implementation of a DCR Graph.
/// </summary>
/// <typeparam name="T"></typeparam>
public class DCRGraph<T> where T : IEvent { 

    // nodes
    private readonly HashSet<T> _events = []; 

    // edges
    private readonly Dictionary<T, HashSet<T>> _conditions = [],
                                               _milestones = [],
                                               _responses = [],
                                               _excludes = [],
                                               _includes = [];
    
    private DCRMarking<T> _marking = new([], [], []);
    
    // simple constructor
    public DCRGraph() { }

    // constructor with full initialization 
    public DCRGraph(HashSet<T> events, 
                    Dictionary<T, HashSet<T>> conditions, 
                    Dictionary<T, HashSet<T>> milestones, 
                    Dictionary<T, HashSet<T>> responses, 
                    Dictionary<T, HashSet<T>> excludes, 
                    Dictionary<T, HashSet<T>> includes,
                    DCRMarking<T> marking) {
        _events = events;
        _conditions = conditions;
        _milestones = milestones;
        _responses = responses;
        _excludes = excludes;
        _includes = includes;
        _marking = marking;
    }

    #region Builder functions

    public void AddEvent(T e) {
        _events.Add(e);
        _conditions[e] = [];
        _milestones[e] = [];
        _responses[e] = [];
        _excludes[e] = [];
        _includes[e] = [];
    }

    // first two are reversed! condition[to] = from
    public void AddCondition (T from, T to) => _conditions[to].Add(from);
    public void AddMilestone (T from, T to) => _milestones[to].Add(from);

    public void AddResponse  (T from, T to) => _responses[from].Add(to);
    public void AddExclude   (T from, T to) => _excludes[from].Add(to);
    public void AddInclude   (T from, T to) => _includes[from].Add(to);

    public void MarkEventAsIncluded(T e) => _marking.Included.Add(e);
    public void MarkEventAsExecuted(T e) => _marking.Executed.Add(e);
    public void MarkEventAsPending (T e) => _marking.Pending.Add(e);

    #endregion

    public bool IsEnabled(T e) {
        // if e is not included, it is not enabled
        if (!_marking.Included.Contains(e)) 
            return false;

        // if e has unexecuted conditions, it is not enabled
        if (_conditions[e].Any(c => !_marking.Executed.Contains(c))) 
            return false;

        // if e has pending milestones, it is not enabled
        if (_milestones[e].Any(m => _marking.Pending.Contains(m))) 
            return false;
        
        // otherwise, it is enabled
        return true;
    }

    public void Execute(T e) {
        throw new NotImplementedException();
    }

    public bool IsAccepting() {
        throw new NotImplementedException();
    }

    public DCRMarking<T> Marking => (DCRMarking<T>)_marking.Clone(); 
} 

