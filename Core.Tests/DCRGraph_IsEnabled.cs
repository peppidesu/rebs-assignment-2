using NUnit.Framework;
using NSubstitute;
namespace Core.Tests;

[TestFixture]
public class DCRGraph_IsEnabled {



    [Test]
    public void IsEnabled_NotIncluded_ReturnsFalse() {
        var a = Substitute.For<IEvent>();

        var graph = new DCRGraph<IEvent>();
        graph.AddEvent(a);

        Assert.IsFalse(graph.IsEnabled(a));
    }
    [Test]
    public void IsEnabled_HasUnexecutedCondition_ReturnsFalse() {

    }
    [Test]
    public void IsEnabled_HasUnexecutedMilestone_ReturnsFalse() {

    }
    [Test]
    public void IsEnabled_HasPendingMilestone_ReturnsFalse() {

    }
    [Test]
    public void IsEnabled_Included_AllConditionsExecuted_NoPendingMilestones_ReturnsTrue() {

    }
    [Test]
    public void IsEnabled_Included_NoConditions_NoMilestones_ReturnsTrue() {

    }
}