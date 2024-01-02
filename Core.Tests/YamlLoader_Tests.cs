

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
    internal bool GraphEquals(DCRGraph lhs, DCRGraph rhs) {
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
        Dictionary<Event,HashSet<Event>> lhs, 
        Dictionary<Event,HashSet<Event>> rhs) {
            return lhs.Order()
                .Zip(rhs.Order())
                .All((pair) => {
                    return pair.First.Key.Equals(pair.Second.Key)
                    && EventsEquals(pair.First.Value, pair.Second.Value);
                });
    }
    internal bool EventsEquals(HashSet<Event> lhs, HashSet<Event> rhs) {
        return lhs.Order().SequenceEqual(rhs.Order());
    }

    [Test]
    public void LoadFromString_UndefinedGivesDefault() {
        var expected = new DCRGraph();
        var actual = _loader.LoadFromString("""
        events: []
        """);
    
        Assert.That(GraphEquals(expected,actual), Is.True);
    }
}