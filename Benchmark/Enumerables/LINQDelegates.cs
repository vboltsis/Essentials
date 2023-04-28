namespace Benchmark;

[MemoryDiagnoser]
public class LINQDelegates
{
    private List<Songs> _songs;

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random();
        _songs = new List<Songs>(100_000);
        for (int i = 0; i < 100_000; i++)
        {
            _songs.Add(new Songs
            {
                Seconds = random.Next(120, 601),
                Name = "Song Name",
                Artist = "Artist Name",
                Album = "Album Name",
                Year = random.Next(2000, 2040),
                Genre = "Genre Name"
            });
        }
    }

    //[Benchmark]
    //public IEnumerable<Songs> GetSongs()
    //{
    //    return _songs.Where(() => s)
    //}
}

public class Songs
{
    public int Seconds { get; set; }
    public string Name { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }
    public int Year { get; set; }
    public string Genre { get; set; }
}