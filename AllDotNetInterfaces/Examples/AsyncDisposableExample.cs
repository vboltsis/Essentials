using System.Text;

namespace AllDotNetInterfaces;

public class AsyncDisposableExample : IAsyncDisposable
{
    private readonly FileStream fileStream;

    public AsyncDisposableExample()
    {
        fileStream = new FileStream("example.txt", FileMode.OpenOrCreate);
    }

    public async Task WriteToFileAsync(string text)
    {
        var bytes = Encoding.UTF8.GetBytes(text);
        await fileStream.WriteAsync(bytes, 0, bytes.Length);
        await fileStream.FlushAsync();
        await DisposeAsync();
    }

    public async Task WriteToFileWithUsingAsync(string text)
    {
        await using var fileStream = new FileStream("example.txt", FileMode.OpenOrCreate);
        var bytes = Encoding.UTF8.GetBytes(text);
        await fileStream.WriteAsync(bytes, 0, bytes.Length);
        await fileStream.FlushAsync();
    }

    private bool disposed = false;

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (disposed)
        {
            return;
        }
        await fileStream.DisposeAsync();
        disposed = true;
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

    ~AsyncDisposableExample()
    {
        Dispose(false);
    }
}
