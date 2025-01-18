using System.Runtime.CompilerServices;

namespace FeatureExamples;

/* USAGE:
 using FeatureExamples;

string filePath = @"C:\Temp\example.txt";
Console.WriteLine("Waiting for the file to be created...");

FileInfo createdFile = await new FileCreatedAwaitable(filePath);

Console.WriteLine("File has been created!");
Console.WriteLine($"File Name: {createdFile.Name}");
Console.WriteLine($"File Size: {createdFile.Length} bytes");
Console.WriteLine($"Created On: {createdFile.CreationTime}");
 */

public class FileCreatedAwaiter : INotifyCompletion
{
    private readonly string _filePath;
    private Action _continuation;
    private FileSystemWatcher _watcher;

    public FileCreatedAwaiter(string filePath)
    {
        _filePath = filePath;
        IsCompleted = File.Exists(_filePath);

        if (!IsCompleted)
        {
            _watcher = new FileSystemWatcher(Path.GetDirectoryName(_filePath))
            {
                Filter = Path.GetFileName(_filePath),
                EnableRaisingEvents = true
            };
            _watcher.Created += OnFileCreated;
        }
    }

    public bool IsCompleted { get; private set; }

    public FileInfo GetResult()
    {
        _watcher?.Dispose();

        return new FileInfo(_filePath);
    }

    public void OnCompleted(Action continuation)
    {
        if (IsCompleted)
        {
            continuation();
        }
        else
        {
            _continuation = continuation;
        }
    }

    private void OnFileCreated(object sender, FileSystemEventArgs e)
    {
        IsCompleted = true;
        _watcher.Dispose();
        _continuation?.Invoke();
    }
}

public class FileCreatedAwaitable
{
    private readonly string _filePath;

    public FileCreatedAwaitable(string filePath)
    {
        _filePath = filePath;
    }

    public FileCreatedAwaiter GetAwaiter()
    {
        return new FileCreatedAwaiter(_filePath);
    }
}
