namespace FeatureExamples;

public readonly struct ServiceResult<T>
{
    public T Data { get; }
    public Exception Error { get; }
    public bool IsSuccess => Error == null;

    public ServiceResult(T data)
    {
        Data = data;
        Error = null;
    }

    public ServiceResult(Exception error)
    {
        Data = default;
        Error = error;
    }

    // Implicit operator for data
    public static implicit operator ServiceResult<T>(T data) => new ServiceResult<T>(data);

    // Implicit operator for exception
    public static implicit operator ServiceResult<T>(Exception ex) => new ServiceResult<T>(ex);
}
