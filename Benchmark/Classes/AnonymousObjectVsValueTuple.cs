namespace Benchmark;

/*
| Method          | Mean       | Error    | StdDev   | Gen0   | Gen1   | Allocated |
|---------------- |-----------:|---------:|---------:|-------:|-------:|----------:|
| AnonymousObject | 1,104.7 ns | 20.89 ns | 21.45 ns | 0.3490 | 0.0019 |   2.85 KB |
| ValueTuple      |   942.9 ns | 15.04 ns | 13.33 ns | 0.3071 | 0.0019 |   2.52 KB |
*/

[MemoryDiagnoser] 
[ReturnValueValidator(true)]
public class AnonymousObjectVsValueTuple
{
    private static readonly Movie[] Movies =
    [
        new Movie { Title = "The Dark Knight", Year = 2008, Director = "Christopher Nolan", Rating = 9.0 },
        new Movie { Title = "The Shawshank Redemption", Year = 1994, Director = "Frank Darabont", Rating = 9.3 },
        new Movie { Title = "Pulp Fiction", Year = 1994, Director = "Quentin Tarantino", Rating = 8.9 },
        new Movie { Title = "Inception", Year = 2010, Director = "Christopher Nolan", Rating = 8.8 },
        new Movie { Title = "Fight Club", Year = 1999, Director = "David Fincher", Rating = 8.8 },
        new Movie { Title = "The Matrix", Year = 1999, Director = "The Wachowski Brothers", Rating = 8.7 },
        new Movie { Title = "Goodfellas", Year = 1990, Director = "Martin Scorsese", Rating = 8.7 },
        new Movie { Title = "Se7en", Year = 1995, Director = "David Fincher", Rating = 8.6 },
        new Movie { Title = "The Silence of the Lambs", Year = 1991, Director = "Jonathan Demme", Rating = 8.6 },
        new Movie { Title = "Star Wars: Episode V - The Empire Strikes Back", Year = 1980, Director = "Irvin Kershner", Rating = 8.7 },
        new Movie { Title = "Forrest Gump", Year = 1994, Director = "Robert Zemeckis", Rating = 8.8 },
        new Movie { Title = "The Lord of the Rings: The Fellowship of the Ring", Year = 2001, Director = "Peter Jackson", Rating = 8.8 },
        new Movie { Title = "The Lord of the Rings: The Return of the King", Year = 2003, Director = "Peter Jackson", Rating = 8.9 },
        new Movie { Title = "The Godfather", Year = 1972, Director = "Francis Ford Coppola", Rating = 9.2 },
        new Movie { Title = "The Godfather: Part II", Year = 1974, Director = "Francis Ford Coppola", Rating = 9.0}
    ];

    [Benchmark]
    public Dictionary<string, IGrouping<string, Movie>> AnonymousObject()
    {
        var movies = Movies.GroupBy(m => m.Director)
            .Select(g => new
            {
                Director = g.Key,
                Movies = g
            })
            .ToDictionary(g => g.Director, g => g.Movies);

        return movies;
    }

    [Benchmark]
    public Dictionary<string, IGrouping<string, Movie>> ValueTuple()
    {
        var movies = Movies.GroupBy(m => m.Director)
            .Select(g =>
            (
                Director: g.Key,
                Movies: g
            ))
            .ToDictionary(g => g.Director, g => g.Movies);

        return movies;
    }
}

public class Movie
{
    public string Title { get; set; }
    public int Year { get; set; }
    public string Director { get; set; }
    public double Rating { get; set; }
}