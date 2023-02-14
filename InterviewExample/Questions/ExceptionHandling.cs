namespace InterviewExample;

public class ExceptionHandling
{
    public static void Method2()
    {
        try
        {
            Method1();
        }
        catch (Exception ex)
        {
            //throw ex resets the stack trace Coming from Method 1 and propagates it to the caller(Main)
            throw ex;
        }
    }

    public static void Method1()
    {
        try
        {
            ThrowError();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace.ToString());
            throw;
        }
    }

    private static void ThrowError()
    {
        throw new Exception("Inside Method1", new Exception("this is the inner message"));
    }
}
