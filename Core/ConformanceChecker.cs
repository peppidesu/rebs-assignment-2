namespace Core;



public class ConformanceChecker {

    public bool IsConformant(ref DCRGraph graph, Queue<Event> log) {
        var initState = graph.Marking;

        while (log.TryDequeue(out Event? e)) {
            try {
                graph.Execute(e);
            }
            catch (ArgumentException) {
                graph.Reset(initState);
                throw;
            }
            catch (EventNotEnabledException) {
                graph.Reset(initState);
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