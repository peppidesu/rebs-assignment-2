namespace Core;



public class ConformanceChecker {

    public bool IsConformant<TEvent>(DCRGraph<TEvent> graph, Queue<TEvent> log) where TEvent : IEvent {
        var initState = graph.Marking;

        while (log.TryDequeue(out TEvent? e)) {
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
        graph.Reset(initState);
        return true;
    }
}