int number = 1;
User user = new User { Name = "John" };

Modify(number, user);

Console.WriteLine(user.Name); //John or Nick?
Console.WriteLine(number); // 1 or 2?

void Modify(int number, User user)
{
    user = new User();
    number = 2;
    user.Name = "Nick";
}

class User
{
    public string Name { get; set; }
    
    //a re-usable method to call external apis with retries
    public static TResult Indefinitely<TResult>(Func<TResult> attemptUnsafely, Action<Exception> errorHandler, int minWaitTimeMillis,
        int maxWaitTimeMillis)
    {
        var retryMillis = minWaitTimeMillis;

        while (true)
        {
            try
            {
                var result = attemptUnsafely();
                return result;
            }
            catch (Exception e)
            {
                errorHandler(e);
                Thread.Sleep(retryMillis);
                retryMillis = Math.Min(retryMillis * 2, maxWaitTimeMillis);
            }
        }
    }
}

class Test
{
    public void Print2(string text)
    {
        
    }
    
    public static void Print(string text)
    {
        Console.WriteLine(text);
    }
}

// interface IInterface
// {
//     void TestMethod()
//     {
//         Console.WriteLine("Hello World!");
//     }
// }
//using var filestream = new FileStream("new SafeFileHandle()", FileMode.Open);

// string text = "hello";
// string world = "world";
// string result = text + " " + world;
//
// StringBuilder builder = new StringBuilder();
// builder.Append("hello");
// builder.Append(" ");
// builder.Append("world");
// string builderResult = builder.ToString();


// for (int i = 0; i < 100_000; i++)
// {
//     await Task.Delay(1);
// }

//
// for (int i = 0; i < 100_000; i++)
// {
//     Task.Run(() =>
//     {
//         //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
//     });
// }
//
// await Task.Delay(5000);
//
//
// Console.ReadKey();


/*
    void Example()
   {
       object obj = new object();

       lock (obj)
       {
           Console.WriteLine("Name");
       }
   }
*/