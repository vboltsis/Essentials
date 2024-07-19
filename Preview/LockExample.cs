namespace Preview;

public class LockExample
{
    private static readonly Lock _lockObj = new();

    public static void Modify(int timeout)
    {
        // Using the C# lock keyword
        lock (_lockObj)
        {
            // Critical section associated with _lockObj
            Console.WriteLine("Lock keyword: Modifying data...");
        }

        // Using EnterScope with the C# using keyword
        using (_lockObj.EnterScope())
        {
            // Critical section associated with _lockObj
            Console.WriteLine("EnterScope: Modifying data...");
        }

        // Using Enter and Exit methods
        _lockObj.Enter();
        try
        {
            // Critical section associated with _lockObj
            Console.WriteLine("Enter method: Modifying data...");
        }
        finally
        {
            _lockObj.Exit();
        }

        // Using TryEnter method
        if (_lockObj.TryEnter(timeout))
        {
            try
            {
                // Critical section associated with _lockObj
                Console.WriteLine("TryEnter method: Modifying data...");
            }
            finally
            {
                _lockObj.Exit();
            }
        }
    }

    public static void ProcessData(int level)
    {
        if (_lockObj.IsHeldByCurrentThread)
        {
            Console.WriteLine($"[{level}] Lock is already held by current thread, processing data...");
            ProcessDataInternal(level);
        }
        else
        {
            using (_lockObj.EnterScope())
            {
                Console.WriteLine($"[{level}] Entered lock, processing data...");
                ProcessDataInternal(level);
            }
        }

        void ProcessDataInternal(int level)
        {
            // Simulate data processing
            Console.WriteLine($"[{level}] Data processed.");

            // Recursively call ProcessData to simulate nested calls
            if (level < 3)
            {
                ProcessData(level + 1);
            }
        }
    }
}