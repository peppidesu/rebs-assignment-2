namespace Core.Tests;

[TestFixture]
public class DCRGraph_Execute {
    [Test]
    public void Execute_ThrowsOnUnknownEvent() {
        var a = Substitute.For<IEvent>();

        var graph = new DCRGraph<IEvent>();    

        Assert.Throws<ArgumentException>(() => graph.Execute(a));
    }

    [Test]
    public void Execute_ThrowsOnUnenabledEvent() {
        var a = Substitute.For<IEvent>();

        var graph = new DCRGraph<IEvent>();    
        graph.AddEvent(a);        

        Assert.Throws<ArgumentException>(() => graph.Execute(a));
    }

    [Test]
    public void Execute_AddsToExecuted() {
        var a = Substitute.For<IEvent>();

        var graph = new DCRGraph<IEvent>();    
        graph.AddEvent(a);        
        graph.MarkEventAsIncluded(a);

        graph.Execute(a);

        Assert.That(graph.Marking.Executed, Has.Member(a));
    }

    [Test]
    public void Execute_RemovesFromPending() {
        var a = Substitute.For<IEvent>();

        var graph = new DCRGraph<IEvent>();    
        graph.AddEvent(a);        
        graph.MarkEventAsIncluded(a);
        graph.MarkEventAsPending(a);

        graph.Execute(a);

        Assert.That(graph.Marking.Pending, Has.No.Member(a));        
    }

    [Test]
    public void Execute_MakeResponsesPending() {
        var a = Substitute.For<IEvent>();
        var b = Substitute.For<IEvent>();

        var graph = new DCRGraph<IEvent>();    
        graph.AddEvent(a);        
        graph.AddEvent(b);        
        graph.MarkEventAsIncluded(a);
        graph.AddResponse(a, b);

        graph.Execute(a);

        Assert.That(graph.Marking.Pending, Has.Member(b));        
    }
    [Test]
    public void Execute_Pending_RemoveBeforeAdd() {
        var a = Substitute.For<IEvent>();        

        var graph = new DCRGraph<IEvent>();    
        graph.AddEvent(a);                
        graph.MarkEventAsIncluded(a);
        graph.MarkEventAsPending(a);
        graph.AddResponse(a, a);

        graph.Execute(a);

        Assert.That(graph.Marking.Pending, Has.Member(a));        
    }

    [Test]
    public void Execute_RemoveExcluded() {
        var a = Substitute.For<IEvent>();
        var b = Substitute.For<IEvent>();

        var graph = new DCRGraph<IEvent>();    
        graph.AddEvent(a);        
        graph.AddEvent(b);        
        graph.MarkEventAsIncluded(a);
        graph.MarkEventAsIncluded(b);
        graph.AddExclude(a, b);

        graph.Execute(a);

        Assert.That(graph.Marking.Included, Has.No.Member(b));        
    }

    [Test]
    public void Execute_AddIncluded() {
        var a = Substitute.For<IEvent>();
        var b = Substitute.For<IEvent>();

        var graph = new DCRGraph<IEvent>();    
        graph.AddEvent(a);        
        graph.AddEvent(b);        
        graph.MarkEventAsIncluded(a);
        graph.AddInclude(a, b);

        graph.Execute(a);

        Assert.That(graph.Marking.Included, Has.Member(b));        
    }

    [Test]
    public void Execute_Included_RemoveBeforeAdd() {
        var a = Substitute.For<IEvent>();        
        var b = Substitute.For<IEvent>();

        var graph = new DCRGraph<IEvent>();    
        graph.AddEvent(a);                
        graph.AddEvent(b);
        graph.MarkEventAsIncluded(a);
        graph.MarkEventAsIncluded(b);
        graph.AddInclude(a, b);
        graph.AddExclude(a, b);

        graph.Execute(a);

        Assert.That(graph.Marking.Included, Has.Member(a));        
    }
}