/*
The Dependency Inversion Principle (DIP) states that high level modules,
should not depend on low level modules; both should depend on abstractions.
Abstractions should not depend on details.
Details should depend upon abstractions.

By using interface we get these benefits:
1. We do not depend on an implementation.
2. We can easily change the implementation.
3. We can easily unit test the code.
4. We instantiate the dependencies only once.
5. The class will not be instantiated unless all the dependencies are provided.
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

    public void CreatePost()
    {
        try
        {
            //create post
        }
        catch (Exception e)
        {
            _logger.LogError();
        }
    }

    public void DeletePost()
    {
        try
        {
            //delete post
        }
        catch (Exception e)
        {
            _logger.LogError();
        }
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

public class LogToFile : Ilogger
{
    public void LogError() { } //--> logs to File
}

public class LogToGraylog : Ilogger
{
    public void LogError() { } //--> logs to Graylog
}