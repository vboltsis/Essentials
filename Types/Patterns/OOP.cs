namespace Types;

//access modifiers public, private, protected, internal, protected internal, file
//abstract class, sealed class, static class

//the default access modifier is internal for class and private for members

//cannot be instantiated
abstract class Animal : IAnimal
{
    public int Color { get; set; }
    protected int Age { get; set; }
    private int Id { get; set; }

    public void Method()
    {
        Console.WriteLine("Method");
    }

    protected void Method2()
    {
        Console.WriteLine("Method2");
    }

    private void Method3()
    {
        Console.WriteLine("Method3");
    }
}

//this is a contract to be implemented by the derived class
internal interface IAnimal
{
    //default interface implementation
    void Feed()
    {
        Console.WriteLine("Hello");
    }
}

//cannot be inherited
sealed class Dog : Animal
{
    public static void Bark()
    {
        Console.WriteLine("Bark");
    }
}
class Cat : Animal
{
    public static void Meow()
    {
        Console.WriteLine("Meow");
    }
}

//cannot create an instance of a static class
//cannot inherit from a static class
static class StaticClass
{
    public static void Method()
    {
        Console.WriteLine("StaticClass");
    }
}
