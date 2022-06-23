/*
The Dependency Inversion Principle (DIP) states that high level modules,
should not depend on low level modules; both should depend on abstractions.
Abstractions should not depend on details.
Details should depend upon abstractions.
 */
public record DependencyInversion
{
    public static void Example()
    {
        var post = new Post(new LogToDB());

        var secondPost = new Post(new LogToFile());
    }
}

public record Post
{
    private readonly Ilogger _logger;

    public Post(Ilogger logger)
    {
        _logger = logger;
    }
}

public interface Ilogger
{
    void LogError();
}

public class LogToDB : Ilogger
{
    public void LogError() { }// --> logs to db
}

class LogToFile : Ilogger
{
    public void LogError() { } //--> logs to File
}
