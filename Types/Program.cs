using System;
using System.Runtime.CompilerServices;

public class Logger
{
    public void Log(string message, [CallerMemberName] string callerName = "")
    {
        Console.WriteLine($"[{callerName}] {message}");
    }
}

class Program
{
    static void Main()
    {
        var logger = new Logger();
        logger.Log("Hello, world!");
        MyMethod(logger);
    }

    static void MyMethod(Logger logger)
    {
        logger.Log("Inside MyMethod");
    }
}