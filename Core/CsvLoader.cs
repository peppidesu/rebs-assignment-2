namespace Core;

/// <summary>
/// Implementation Csv Loader
/// </summary>
public class CsvLoader
{
    // TODO: pass `path` as an argument here
    public Dictionary<string, Queue<string>> LoadCsv(string path)
    {
        // Create Dict
        Dictionary<string, Queue<string>> idTitleMap = new Dictionary<string, Queue<string>>();

        
        using (StreamReader reader = new StreamReader(path))
        {
            while (!reader.EndOfStream)
            {
                
                string line = reader.ReadLine()!;
                string[] columns = line.Split(';');

                // Grab relevant columns
                string id = columns[0];
                string title = columns[2];

                // Check if ID is already in the dictionary
                if (idTitleMap.TryGetValue(id, out Queue<string>? titleQueue)) 
                {
                    // Add the title to the existing queue
                    titleQueue.Enqueue(title);
                }
                else
                {
                    // Create a new queue for the ID and add the title
                    titleQueue = new Queue<string>();
                    titleQueue.Enqueue(title);
                    idTitleMap.Add(id, titleQueue);
                }

            }
        }

        return idTitleMap;
    }
}

/// <summary>
/// Usage Csv Loader
/// </summary>
/* class CSVLoaderUsage
{
    static void Main()
    {
        // Example usage
        string csvFilePath = "../logs/log.csv";
        CsvLoader csvLoader = new CsvLoader();
        Dictionary<string, Queue<string>> idTitleMap = csvLoader.LoadCsv(csvFilePath);

        // Display the loaded CSV data
        foreach (var entry in idTitleMap)
        {
            string id = entry.Key;
            Queue<string> titles = entry.Value;

            Console.WriteLine($"ID: {id}, Titles: {string.Join(", ", titles)}");
        }
    }
} */
