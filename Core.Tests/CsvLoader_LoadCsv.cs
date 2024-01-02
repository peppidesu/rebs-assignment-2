namespace Core.Tests;

[TestFixture]
public class CsvLoaderTests
{
    [Test]
    public void LoadCsv_WithValidCsvData_ReturnsCorrectDictionary()
    {
        // Test data
        string csvData = "\n" +
                         "1;Event1;Title1;Role1;Date\n" +
                         "1;Event2;Title2;Role1;Date\n" +
                         "2;Event3;Title3;Role1;Date\n" +
                         "2;Event4;Title4;Role1;Date";

        // Temp file to pretend were reading from one
        string tempFilePath = Path.GetTempFileName();
        File.WriteAllText(tempFilePath, csvData);

        CsvLoader csvLoader = new CsvLoader();

        // Do the thing
        Dictionary<string, Queue<Event>> idTitleMap = csvLoader.LoadCsv(tempFilePath);

        // Tests
        Assert.That(idTitleMap, Has.Count.EqualTo(2));
        Assert.That(idTitleMap["1"], Has.Count.EqualTo(2));
        Assert.That(idTitleMap["1"].Dequeue().Id, Is.EqualTo("Title1"));
        Assert.That(idTitleMap["1"].Dequeue().Id, Is.EqualTo("Title2"));

        
        Assert.That(idTitleMap.ContainsKey("2"), Is.True);
        Assert.That(idTitleMap["2"], Has.Count.EqualTo(2));
        Assert.That(idTitleMap["2"].Dequeue().Id, Is.EqualTo("Title3"));
        Assert.That(idTitleMap["2"].Dequeue().Id, Is.EqualTo("Title4"));
        
    }
}