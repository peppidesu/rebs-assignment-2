namespace Core;

/// <summary>
/// Implementation Csv Loader
/// </summary>
public class CsvLoader
{
    // TODO: pass `path` as an argument here
    public Dictionary<string, Queue<LogEvent>> LoadCsv(string path)
    {
        // Create Dict
        var result = new Dictionary<string, Queue<LogEvent>>();

        
        using (StreamReader reader = new StreamReader(path))
        {
            reader.ReadLine(); // discard title row
            while (!reader.EndOfStream)
            {
                
                string line = reader.ReadLine()!;
                string[] columns = line.Split(';');

                // Grab relevant columns
                string run = columns[0];
                string title = columns[2];
                string date = columns[4];

                // Check if ID is already in the dictionary
                if (result.TryGetValue(run, out Queue<LogEvent>? titleQueue)) 
                {
                    // Add the title to the existing queue
                    titleQueue.Enqueue(new LogEvent(title, run, date));
                }
                else
                {
                    // Create a new queue for the ID and add the title
                    titleQueue = new Queue<LogEvent>();
                    titleQueue.Enqueue(new LogEvent(title, run, date));
                    result.Add(run, titleQueue);
                }

            }
        }

        return result;
    }
}