namespace Core;



public class ConformanceChecker {

    public bool IsConformant(ref DCRGraph graph, Queue<LogEvent> log) {
        var initState = graph.Marking;

        while (log.TryDequeue(out LogEvent? e)) {
            try {
                graph.Execute(e);
            }
            catch (ArgumentException) { // event not found in graph
                graph.Reset(initState); 
                Output.Trace($"=== Run '{run}' failed at event '{e}': event doesn't exist in the graph. ===");
                return false;
            }
            catch (EventNotEnabledException) {
                graph.Reset(initState);
                Output.Trace($"Run '{e.Run}' failed at event '{e}': event is not enabled.");
                return false;
            }
        }
        if (!graph.IsAccepting()) {
            graph.Reset(initState);
            return false;
        }
        
        graph.Reset(initState);
        return true;
    }
}