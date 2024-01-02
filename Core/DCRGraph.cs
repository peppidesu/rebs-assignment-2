namespace Core;

/// <summary>
/// Implementation of a DCR Graph.
/// </summary>
public class DCRGraph { 

    // nodes
    private readonly HashSet<Event> _events = []; 
    public HashSet<Event> Events => new(_events);
    
    // edges
    private readonly Dictionary<Event, HashSet<Event>> _conditions = [],
                                               _milestones = [],
                                               _responses = [],
                                               _excludes = [],
                                               _includes = [];
    
    public Dictionary<Event, HashSet<Event>> Conditions => new(_conditions);
    public Dictionary<Event, HashSet<Event>> Milestones => new(_milestones);
    public Dictionary<Event, HashSet<Event>> Responses => new(_responses);
    public Dictionary<Event, HashSet<Event>> Excludes => new(_excludes);
    public Dictionary<Event, HashSet<Event>> Includes => new(_includes);
    
    private DCRMarking _marking = new([], [], []);
    public DCRMarking Marking => (DCRMarking)_marking.Clone(); 
    
    // simple constructor
    public DCRGraph() { }

    // constructor with full initialization 
    public DCRGraph(HashSet<Event> events, 
                    Dictionary<Event, HashSet<Event>> conditions, 
                    Dictionary<Event, HashSet<Event>> milestones, 
                    Dictionary<Event, HashSet<Event>> responses, 
                    Dictionary<Event, HashSet<Event>> excludes, 
                    Dictionary<Event, HashSet<Event>> includes,
                    DCRMarking marking) {
        _events = events;
        _conditions = conditions;
        _milestones = milestones;
        _responses = responses;
        _excludes = excludes;
        _includes = includes;
        _marking = marking;
    }

    #region Builder functions

    public void AddEvent(Event e) {
        _events.Add(e);
        _conditions[e] = [];
        _milestones[e] = [];
        _responses[e] = [];
        _excludes[e] = [];
        _includes[e] = [];
    }

    // first two are reversed! condition[to] = from

    public void AddCondition (Event from, Event to) => _conditions[to].Add(from);
    public void AddMilestone (Event from, Event to) => _milestones[to].Add(from);

    public void AddResponse  (Event from, Event to) => _responses[from].Add(to);
    public void AddExclude   (Event from, Event to) => _excludes[from].Add(to);
    public void AddInclude   (Event from, Event to) => _includes[from].Add(to);

    public void MarkEventAsIncluded(Event e) => _marking.Included.Add(e);
    public void MarkEventAsExecuted(Event e) => _marking.Executed.Add(e);
    public void MarkEventAsPending (Event e) => _marking.Pending.Add(e);

    #endregion

    /// <summary>
    /// Checks if an event is enabled in the current marking.
    /// </summary>
    /// <param name="e">The event to check.</param>
    /// <returns></returns>
    public bool IsEnabled(Event e) {
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

    public void Execute(Event e) {
        //check if event exists
        if (!_events.Contains(e)) 
            throw new ArgumentException($"Event '{e}' is not defined in this graph.");

        //check if event is enabled
        if(!IsEnabled(e))
            throw new EventNotEnabledException($"Event '{e}' is not enabled");

        //event is added to the executed set
        _marking.Executed.Add(e);

        //event is removed from the pending set
        _marking.Pending.Remove(e); 

        //all the events that e makes pending are added to the pending set
        _marking.Pending.UnionWith(_responses[e]);

        //all the events that e excludes are removed from the included set
        _marking.Included.ExceptWith(_excludes[e]);

        //all the events that e includes are added to the included set
        _marking.Included.UnionWith(_includes[e]);

    }

    public bool IsAccepting() {
        // Check if there are no included pending events in the marking
        if (_marking.Pending.Any(pendingEvent => _marking.Included.Contains(pendingEvent)))
            return false;
        return true;
    }

    public void Reset(DCRMarking marking) {
        _marking = marking;
    }
} 

public class EventNotEnabledException(string message) : Exception(message) {}
 
