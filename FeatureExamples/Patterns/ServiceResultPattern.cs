namespace FeatureExamples;

public readonly struct Result<T>
{
    public T Data { get; }
    public Exception Error { get; }
    public bool IsSuccess => Error == null;

    public Result(T data)
    {
        Data = data;
        Error = null;
    }
    
    public Result(Exception error)
    {
        Data = default;
        Error = error;
    }

    // Implicit operator for data
    public static implicit operator Result<T>(T data) => new Result<T>(data);

    // Implicit operator for exception
    public static implicit operator Result<T>(Exception ex) => new Result<T>(ex);
}
