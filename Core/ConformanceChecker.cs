namespace Core;
public class ConformanceChecker {

    /// <summary>
    /// Checks whether a given log is conformant with the given graph.
    /// The graph marking will be reset after simulating the event log.
    /// </summary>
    /// <param name="graph">The DCR graph to check conformance with</param>
    /// <param name="log">The event log to simulate</param>
    /// <returns>Whether or not the graph and log are conformant.</returns>
    public bool IsConformant(ref DCRGraph graph, Queue<LogEvent> log) {
        var initState = graph.Marking; // keep a copy of initState to reset graph afterwards
        var run = log.Peek().Run; // hacky way to get the run ID
        Output.Trace("=== Running run {run} ... ===");
        while (log.TryDequeue(out LogEvent? e)) {
            try {
                graph.Execute(e);
            }
            catch (ArgumentException) { // event not found in graph
                graph.Reset(initState); 
                Output.Trace($"=== Run '{run}' failed at event '{e}': event doesn't exist in the graph. ===");
                return false;
            }
            catch (EventNotEnabledException) { // event not enabled
                graph.Reset(initState);
                Output.Trace($"=== Run '{run}' failed at event '{e}': event is not enabled. ===");
                return false;
            }
        }
        if (!graph.IsAccepting()) { // graph is not accepting
            graph.Reset(initState);
            Output.Trace($"=== Run '{run}' failed: graph is not accepting. ===");
            return false;
        }
        graph.Reset(initState);
        Output.Trace($"=== Run '{run}' Successful ===");
        return true;
    }
}