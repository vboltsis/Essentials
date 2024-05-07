namespace CSharpFundamentals;

public class Operations
{
    public void ArithmeticOperations()
    {
        int a = 10;
        int b = 3;

        Console.WriteLine(a + b); // 13 addition
        Console.WriteLine(a - b); // 7 subtraction
        Console.WriteLine(a * b); // 30 multiplication
        Console.WriteLine(a / b); // 3 division
        Console.WriteLine(a % b); // 1 remainder (modulo operator)
    }

    public void IncrementDecrement()
    {
        int a = 1;
        int b = a++; // b = 1, a = 2
        int c = ++a; // c = 3, a = 3

        Console.WriteLine($"a: {a}, b: {b}, c: {c}");
    }

    public void ComparisonOperators()
    {
        int a = 3;
        int b = 2;

        Console.WriteLine(a > b); // True
        Console.WriteLine(a < b); // False
        Console.WriteLine(a >= b); // True
        Console.WriteLine(a <= b); // False
        Console.WriteLine(a == b); // False
        Console.WriteLine(a != b); // True
    }

    public void LogicalOperators()
    {
        bool a = true;
        bool b = false;

        Console.WriteLine(a && b); // False
        Console.WriteLine(a || b); // True
        Console.WriteLine(!a); // False
    }

    public void TernaryOperator()
    {
        int a = 1;
        int b = 2;

        int result = a > b ? a : b;

        Console.WriteLine(result); // 2
    }

    public void BitwiseOperators()
    {
        int a = 5; // 101
        int b = 3; // 011

        Console.WriteLine(a & b); // 1 (AND)
        Console.WriteLine(a | b); // 7 (OR)
        Console.WriteLine(a ^ b); // 6 (XOR)
        Console.WriteLine(~a); // -6 (NOT)
        Console.WriteLine(a << 1); // 10 (left shift)
        Console.WriteLine(a >> 1); // 2 (right shift)
    }

    public void AssignmentOperators()
    {
        int a = 1;
        a += 2; // a = a + 2
        a -= 1; // a = a - 1
        a *= 3; // a = a * 3
        a /= 2; // a = a / 2
        a %= 2; // a = a % 2

        Console.WriteLine(a); // 1
    }

    public void NullCoalescingOperator()
    {
        int? a = null;
        int b = a ?? 0;

        Console.WriteLine(b); // 0
    }

    public void NullConditionalOperator()
    {
        Person person = null;
        string name = person?.Name;

        Console.WriteLine(name); // null
    }

    public void IsOperator()
    {
        object[] values = { 1, "John", DateTime.Now, null };

        foreach (var value in values)
        {
            if (value is int)
            {
                Console.WriteLine($"{value} is an integer");
            }
            else if (value is string)
            {
                Console.WriteLine($"{value} is a string");
            }
            else if (value is DateTime)
            {
                Console.WriteLine($"{value} is a DateTime");
            }
            else
            {
                Console.WriteLine($"{value} is null");
            }
        }
    }

    public void AsOperator()
    {
        object[] values = { 1, "John", DateTime.Now, null };

        foreach (var value in values)
        {
            if (value is string text)
            {
                Console.WriteLine($"{text} is a string");
            }
        }
    }

    public void SwitchStatement()
    {
        int number = 1;

        switch (number)
        {
            case 1:
                Console.WriteLine("One");
                break;
            case 2:
                Console.WriteLine("Two");
                break;
            default:
                Console.WriteLine("Other");
                break;
        }
    }

    public void SwitchExpression()
    {
        int number = 1;

        string result = number switch
        {
            1 => "One",
            2 => "Two",
            _ => "Other"
        };

        Console.WriteLine(result);
    }



}
