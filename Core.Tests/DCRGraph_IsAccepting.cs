namespace Core.Tests;

[TestFixture]
public class DCRGraph_IsAccepting {
    
        [Test]
        public void IsAccepting_NoIncludedPendingEvents_ReturnsTrue() {
            var a = Substitute.For<Event>();
            var b = Substitute.For<Event>();
            a.Id.Returns("a");
            b.Id.Returns("b");
            var graph = new DCRGraph();

            graph.AddEvent(a);
            graph.AddEvent(b);
            graph.MarkEventAsIncluded(a);
            graph.MarkEventAsPending(b);

            Assert.That(graph.IsAccepting(), Is.True);
        }
        [Test]
        public void IsAccepting_IncludedPendingEvents_ReturnsFalse() {
            var a = Substitute.For<Event>();            

            var graph = new DCRGraph();

            graph.AddEvent(a);            
            graph.MarkEventAsIncluded(a);            
            graph.MarkEventAsPending(a);

            Assert.That(graph.IsAccepting(), Is.False);
        }
}