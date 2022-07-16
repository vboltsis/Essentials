namespace InterviewExample;

internal class Fibonacci
{
    //fibonacci recursion
    public static int FibonacciRecursion(int n)
    {
        if (n == 0)
            return 0;
        if (n == 1)
            return 1;
        
        return FibonacciRecursion(n - 1) + FibonacciRecursion(n - 2);
    }

    //fibonacci no recursion
    public static int FibonacciNoRecursion(int n)
    {
        if (n == 0)
            return 0;

        if (n == 1)
            return 1;

        int a = 0;
        int b = 1;
        int c = 0;
        for (int i = 1; i < n; i++)
        {
            c = a + b;
            a = b;
            b = c;
        }
        
        return c;
    }
}
