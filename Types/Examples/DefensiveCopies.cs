namespace FeatureExamples
{
    internal class DefensiveCopies
    {
    }
}
/*
 public struct Num
{
    public int Value;

    public Num(int value)
    {
        Value = value;
    }

    public void Increment()
    {
        Value++;
    }

    public override string ToString() => $"Value = {Value.ToString()}";
}
As you can see, it is a simple Num struct with:

An int field called Value .
A constructor.
A method called Increment which increases Value field by 1.
A ToString method override.
Now, let’s say that we have the following code:

public class MainProgram
{
    private Num _number = new Num(1);

    public void Run()
    {
        Console.WriteLine("Before Increment: " + _number.ToString());
        _number.Increment();
        Console.WriteLine("After Increment: " + _number.ToString());
    }
}
In this class, we just defined a private field of type Num,
and in the Run method we just increment the field by calling its own Increment method.
 
output:
1
2

Let’s modify the code to be as follows:

 public class MainProgram
{
    private readonly Num _number = new Num(1);

    public void Run()
    {
        Console.WriteLine("Before Increment: " + _number.ToString());
        _number.Increment();
        Console.WriteLine("After Increment: " + _number.ToString());
    }
}

output:
1
1

A Glimpse Under The Hood
What actually happened is a Defensive Copy.

What happened could be broken down into simple steps:

When we marked the field with the readonly keyword, this revealed our intention to keep this field totally untouched. In other words, we don’t want any changes to be applied to the object behind this field.
Therefore, the compiler actually listened to us and understood our intentions. Thus, the compiler decided to help us achieve our goal.
Then, we tried to increment the field by calling its own Increment method.
Thus, this is where the compiler decided to kick in and protect our field object from any changes even if these changes are triggered from inside itself. But how would the compiler do it?
The compiler would do it by first creating a copy of the field object and then applying the Increment call on it, not on the original field object.
Worth mentioning here is that the field object is of the type Num which is a struct. As we know, copying a struct would yield a totally new object.
Therefore, this would eventually protect our field object from any changes.
So, in simple words, this code:

public class MainProgram
{
    private readonly Num _number = new Num(1);

    public void Run()
    {
        Console.WriteLine("Before Increment: " + _number.ToString());
        _number.Increment();
        Console.WriteLine("After Increment: " + _number.ToString());
    }
}
Would eventually be translated into this code:

public class MainProgram
{
    private readonly Num _number = new Num(1);

    public void Run()
    {
        var number = _number;
        Console.WriteLine("Before Increment: " + number.ToString());

        number = _number;
        number.Increment();

        number = _number;
        Console.WriteLine("After Increment: " + number.ToString());
    }
}
Now you might ask:

But why would the compiler create a copy of the field object before the ToString call?!! This call is not expected to change the object by any means.

Yes, you are right. It is not expected to apply any changes on the field object but this is not the way the compiler thinks.

The compiler doesn’t inspect the code inside the method and decides if it is going to apply any changes to the object or not.

It just assumes that this could happen and this is good enough for the compiler to be cautious and apply the Defensive Copy mechanism.



Defensive Copy Mechanism
Now, let’s go through the questions most probably you have in mind right now.

- When does it happen?
It happens when a struct object is used in a read-only context, and this object is manipulated.

- What do you mean by manipulated?
Manipulated means calling any method on the object. Also, calling a property is the same because the property is a method at the end. However, calling a field would not trigger the mechanism.

- What do you mean by read-only context?
It means when the object is declared as one of the following:

- readonly field

public class MainProgram
{
    private readonly Num _number = new Num(1);

    public void Run()
    {
        Console.WriteLine("Before Increment: " + _number.ToString());
        _number.Increment();
        Console.WriteLine("After Increment: " + _number.ToString());
    }
}
- ref readonly local variable

public class MainProgram
{
    private Num _number = new Num(1);

    public void Run()
    {
        ref readonly Num number = ref _number;

        Console.WriteLine("Before Increment: " + number.ToString());
        number.Increment();
        Console.WriteLine("After Increment: " + number.ToString());
    }
}
- in parameter

public class MainProgram
{
    public void Run(in Num number)
    {
        Console.WriteLine("Before Increment: " + number.ToString());
        number.Increment();
        Console.WriteLine("After Increment: " + number.ToString());
    }
}
- Does it really matter? Should we be concerned about whether Defensive Copy is triggered or not?
It actually depends on the size of the struct and how frequently the Defensive Copy mechanism is triggered.

- The bigger the struct is, the more impact we should expect.

- The more frequently the Defensive Copy mechanism is triggered, the more impact we should expect.


The fix is as simple as marking the struct as readonly.

When we mark the struct as readonly, the compiler makes sure it is immutable. This means that no one can manipulate the object or change its state.

Accordingly, the compiler gets confident that no calls would manipulate the object, and therefore no Defensive Copy mechanism is needed.

Means of Detection
Now, you might be asking:

Is there any tool to help me spot Defensive Copy occurrence?

Yes, ErrorProne.NET Structs nuget package.

It is a group of analyzers that help avoid struct and readonly reference performance pitfalls. Using this nuget package you can spot struct-related problems so that you can fix them on the spot.

What you need to keep in mind is that the analyzers emit a diagnostic only when the struct size is >= 16 bytes.

It was said that you can change this threshold using .editorconfig file and adding the following line:

error_prone.large_struct_threshold = {new threshold}
 */