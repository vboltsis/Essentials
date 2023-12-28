var games = new List<Game>
{
    new Game { Name = "Super Mario Bros.", Code = 1 },
    new Game { Name = "The Legend of Zelda", Code = 2 },
    new Game { Name = "Metroid", Code = 3 },
    new Game { Name = "Donkey Kong", Code = 4 },
};

var result = FilterGames(games);

foreach (var game in result)
{
    Console.WriteLine(game.Name);
}

Console.WriteLine();

static IEnumerable<Game> FilterGames(IEnumerable<Game> games)
{
    var filteredGames = games.Where(g => g.Code > 2).Select(c => new Game
    {
        Code = c.Code,
        Name = c.Name
    });

    foreach (var game in filteredGames)
    {
        game.Name += " 2";
    }

    return filteredGames;
}

class Game
{
    public string Name { get; set; }
    public int Code { get; set; }
}