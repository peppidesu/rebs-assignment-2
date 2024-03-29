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
    // These functions are used for testing in order to create graphs
    // in a deterministic manner.
    
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
    
    /// <summary>
    /// Execute an event
    /// </summary>
    /// <param name="e">the event to execute</param>
    /// <exception cref="ArgumentException">Event is not part of the graph.</exception>
    /// <exception cref="EventNotEnabledException">Event is not enabled.</exception>
    public void Execute(Event e) {
        //check if event exists
        if (!_events.Contains(e)) 
            throw new ArgumentException($"Event '{e}' is not defined in this graph.");

        //check if event is enabled
        if(!IsEnabled(e))
            throw new EventNotEnabledException($"Event '{e}' is not enabled");

        //event is added to the executed set
        _marking.Executed.Add(e);
        Output.Trace($"Executed event '{e}'");
            

        //event is removed from the pending set
        if (_marking.Pending.Remove(e))
            Output.Trace($"- '{e}' is no longer pending");

        //all the events that e makes pending are added to the pending set
        _marking.Pending.UnionWith(_responses[e]);
        if (_responses[e].Count > 0)
            Output.Trace($"- Marked '{string.Join("', '", _responses[e])}' as pending.");

        //all the events that e excludes are removed from the included set
        _marking.Included.ExceptWith(_excludes[e]);
        if (_excludes[e].Count > 0)
            Output.Trace($"- Excluded '{string.Join("', '", _excludes[e])}'");

        //all the events that e includes are added to the included set
        _marking.Included.UnionWith(_includes[e]);
        if (_includes[e].Count > 0)
            Output.Trace($"- Included '{string.Join("', '", _includes[e])}'");

    }

    /// <summary>
    /// Checks whether an event is accepting
    /// </summary>
    /// <returns>true if the graph is accepting, false otherwise.</returns>
    public bool IsAccepting() {
        // Check if there are no included pending events in the marking
        if (_marking.Pending.Any(_marking.Included.Contains))
            return false;
        return true;
    }

    /// <summary>
    /// Reset the graph to a given marking
    /// </summary>
    /// <param name="marking">The marking to reset the graph to.</param>
    public void Reset(DCRMarking marking) {
        _marking = marking;
    }
} 

// Exceptions
public class EventNotEnabledException(string message) : Exception(message) {}
 
