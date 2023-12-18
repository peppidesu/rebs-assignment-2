

namespace Core.Tests;
[TestFixture]
public class YamlLoader_Tests {
    
    YamlLoader _loader;

    [SetUp]
    public void Setup() {
        _loader = new YamlLoader();
    }

    [Test]
    public void DummyTest() {
        var graph = _loader.LoadFromString("""
        events:
        - "a"
        - "b"
        - "c"

        edges:
          - type: "condition"
            from: "a"
            to: "b"
        
        marking:
          included: ["a", "b"]
          pending: ["b"]
          
        """);        
        
        Console.WriteLine(graph.ToString());
    }

    [Test]
    public void BuildFromData_ForbidDuplicateEvents() {
        var graphData = new DCRGraphData {
            events = ["a", "a", "b"]
        };

        Assert.Throws<YamlLoaderException>(() => _loader.BuildFromData(graphData));
    }
    
    /// <summary>
    /// WARNING!! THIS FUNCTION IS VERY SLOW!
    /// </summary>
    /// <returns></returns>
    internal bool GraphEquals(DCRGraph<StringEvent> lhs, DCRGraph<StringEvent> rhs) {
        return EventsEquals(lhs.Events, rhs.Events)
            && EdgesEquals(lhs.Conditions, rhs.Conditions)
            && EdgesEquals(lhs.Milestones, rhs.Milestones)
            && EdgesEquals(lhs.Responses, rhs.Responses)
            && EdgesEquals(lhs.Includes, rhs.Includes)
            && EdgesEquals(lhs.Excludes, rhs.Excludes)
            && EventsEquals(lhs.Marking.Included, rhs.Marking.Included)
            && EventsEquals(lhs.Marking.Executed, rhs.Marking.Executed)
            && EventsEquals(lhs.Marking.Pending, rhs.Marking.Pending);
    }

    internal bool EdgesEquals(
        Dictionary<StringEvent,HashSet<StringEvent>> lhs, 
        Dictionary<StringEvent,HashSet<StringEvent>> rhs) {
            return lhs.Order()
                .Zip(rhs.Order())
                .All((pair) => {
                    return pair.First.Key.Equals(pair.Second.Key)
                    && EventsEquals(pair.First.Value, pair.Second.Value);
                });
    }
    internal bool EventsEquals(HashSet<StringEvent> lhs, HashSet<StringEvent> rhs) {
        return lhs.Order().SequenceEqual(rhs.Order());
    }

    [Test]
    public void LoadFromString_EmptyGivesDefault() {
        var expected = new DCRGraph<StringEvent>();
        var actual = _loader.LoadFromString("");
    
        Assert.That(GraphEquals(expected,actual), Is.True);
    }
}