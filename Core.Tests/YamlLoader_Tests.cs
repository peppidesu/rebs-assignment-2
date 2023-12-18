

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
    
    public void LoadFromString_EmptyGivesDefault() {
        var graph = _loader.LoadFromString("");
        
    }
}