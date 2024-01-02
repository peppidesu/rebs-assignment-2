
namespace Core.Tests;

[TestFixture]
public class DCRGraph_IsEnabled {



    [Test]
    public void IsEnabled_NotIncluded_ReturnsFalse() {
        var a = Substitute.For<Event>();

        var graph = new DCRGraph();
        graph.AddEvent(a);

        Assert.That(graph.IsEnabled(a), Is.False);
    }
    [Test]
    public void IsEnabled_HasUnexecutedCondition_ReturnsFalse() {
        var a = Substitute.For<Event>();
        var b = Substitute.For<Event>();

        var graph = new DCRGraph();
        
        graph.AddEvent(a);
        graph.AddEvent(b);
        graph.MarkEventAsIncluded(b);
        graph.AddCondition(a, b);

        Assert.That(graph.IsEnabled(b), Is.False);
    }
    
    [Test]
    public void IsEnabled_HasPendingMilestone_ReturnsFalse() {
        var a = Substitute.For<Event>();
        var b = Substitute.For<Event>();

        var graph = new DCRGraph();
        
        graph.AddEvent(a);
        graph.AddEvent(b);
        graph.MarkEventAsPending(a);
        graph.MarkEventAsIncluded(b);
        graph.AddMilestone(a, b);

        Assert.That(graph.IsEnabled(b), Is.False);
    }
    [Test]
    public void IsEnabled_Included_AllConditionsExecuted_NoPendingMilestones_ReturnsTrue() {
        var a = Substitute.For<Event>();
        var b = Substitute.For<Event>();
        var c = Substitute.For<Event>();

        var graph = new DCRGraph();
        
        graph.AddEvent(a);
        graph.AddEvent(b);
        graph.AddEvent(c);
        graph.MarkEventAsIncluded(c);
        graph.MarkEventAsExecuted(a);        
        graph.AddCondition(a, c);
        graph.AddMilestone(b, c);

        Assert.That(graph.IsEnabled(c), Is.True);
    }
    [Test]
    public void IsEnabled_Included_NoConditions_NoMilestones_ReturnsTrue() {
        var a = Substitute.For<Event>();

        var graph = new DCRGraph();
        graph.AddEvent(a);
        graph.MarkEventAsIncluded(a);

        Assert.That(graph.IsEnabled(a), Is.True);
    }
}