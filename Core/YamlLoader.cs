using YamlDotNet.Serialization;
namespace Core;

public class YamlLoader {
    private IDeserializer _deserializer;

    public YamlLoader() {
        _deserializer = new DeserializerBuilder().Build();
    }

    public DCRGraph<StringEvent> LoadFromFile(string path) {
        var yaml = File.ReadAllText(path);
        return LoadFromString(yaml);
    }

    public DCRGraph<StringEvent> LoadFromString(string yaml) {
        var graphData = _deserializer.Deserialize<DCRGraphData>(yaml);

        // remove null arrays
        graphData.events ??= [];
        graphData.edges ??= [];
        graphData.marking.included ??= [];
        graphData.marking.executed ??= [];
        graphData.marking.pending ??= [];

        return BuildFromData(graphData);        
    }


    private static HashSet<StringEvent> EventSetFromStringArray(string[] arr) {
        return new HashSet<StringEvent>(arr.Select(e => new StringEvent(e)));
    }      

    public DCRGraph<StringEvent> BuildFromData(DCRGraphData data) {
        var events = EventSetFromStringArray(data.events);
        
        if (data.events.Length > events.Count) {
            var dup = events.First(a => data.events.Count(b => a.Name == b) > 1);
            throw new YamlLoaderException($"Duplicate event '{dup.Name}' in events list");
        }        
        
        Dictionary<StringEvent, HashSet<StringEvent>> 
            conditions = [],
            milestones = [],
            responses = [],
            excludes = [],
            includes = [];        

        foreach (var e in events) {
            conditions.Add(e, []);
            milestones.Add(e, []);
            responses.Add(e, []);
            excludes.Add(e, []);
            includes.Add(e, []);
        }
        
        foreach (var edge in data.edges) {
            // stupid unreachable dummy values because c# does not have errors 
            // as values or some other way to do this better
            StringEvent from = default,
                        to = default;

            // we probably want proper line + column errors here but this does 
            // the job well enough
            try {
                from = events.First(e => e.Name == edge.from);                        
            }
            catch (InvalidOperationException) {
                throw new YamlLoaderException(
                    $"Reference to unknown event '{from}' in edge '{edge}'"
                );
            }

            try {
                to = events.First(e => e.Name == edge.to);
            }
            catch (InvalidOperationException) {
                throw new YamlLoaderException(
                    $"Reference to unknown event '{to}' in edge '{edge}'"
                );
            }
                    
            // wish i had rusts enum structs rn
            switch (edge.type) {
                
                case "condition":
                    conditions[to].Add(from);
                    break;
                case "milestone":
                    milestones[to].Add(from);
                    break;
                case "response":
                    responses[from].Add(to);
                    break;
                case "exclude":
                    excludes[from].Add(to);
                    break;
                case "include":
                    includes[from].Add(to);
                    break;
                default:
                    throw new YamlLoaderException(
                        $"Unknown edge type: {edge.type}"
                    );
            }


        }

        var executed = EventSetFromStringArray(data.marking.executed);
        var included = EventSetFromStringArray(data.marking.included);
        var pending = EventSetFromStringArray(data.marking.pending);

        {
            var diff = executed.Except(events);
            if (diff.Any()) {
                throw new YamlLoaderException($"Reference to unknown event '{diff.First().Name}' in execuded list");
            }
        }
        
        {
            var diff = included.Except(events);
            if (diff.Any()) {
                throw new YamlLoaderException($"Reference to unknown event '{diff.First().Name}' in included list");
            }
        }

        {
            var diff = pending.Except(events);
            if (diff.Any()) {
                throw new YamlLoaderException($"Reference to unknown event '{diff.First().Name}' in pending list");
            }
        }


        var marking = new DCRMarking<StringEvent>(executed, included, pending);

        return new DCRGraph<StringEvent>(events, conditions, milestones, responses, excludes, includes, marking);
    }
}

// i want rust enum structs
public class YamlLoaderException : Exception {
    public YamlLoaderException(string message) : base(message) {}
}

public struct DCRGraphData {
    public string[] events;
    public DCREdgeData[] edges;
    public DCRMarkingData marking;
}

public struct DCREdgeData {
    public string type;
    public string from;
    public string to;

    public override readonly string ToString() => $"({type}): {from} -> {to}";
}
public struct DCRMarkingData {
    public string[] executed;
    public string[] included;
    public string[] pending;
}