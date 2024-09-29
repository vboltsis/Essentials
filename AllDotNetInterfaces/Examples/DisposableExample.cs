using System.Text;

namespace AllDotNetInterfaces;

public class DisposableExample : IDisposable
{
    private readonly FileStream fileStream;

    public DisposableExample()
    {
        fileStream = new FileStream("example.txt", FileMode.OpenOrCreate);
    }

    public void WriteToFile(string text)
    {
        var bytes = Encoding.UTF8.GetBytes(text);
        fileStream.Write(bytes, 0, bytes.Length);

        fileStream.Flush();

        Dispose();
    }

    public void WriteToFileWithUsing(string text)
    {
        using var fileStream = new FileStream("example.txt", FileMode.OpenOrCreate);
        var bytes = Encoding.UTF8.GetBytes(text);
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Flush();
    }

    private bool disposed = false;
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
        {
            return;
        }

        if (disposing)
        {
            fileStream.Dispose();
        }

        disposed = true;
    }

    ~DisposableExample()
    {
        Dispose(false);
    }
}
