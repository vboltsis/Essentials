namespace Types;

public interface IContravariant<in T>
{
    void Display(T item);
}

public class SampleContravariant : IContravariant<Base>
{
    public void Display(Base item)
    {
        Console.WriteLine(item.GetType().Name);
    }
}

public class Base { }
public class Derived : Base { }

public class RunContravariant
{
    public static void Method()
    {
        IContravariant<Derived> contravariantInstance = new SampleContravariant();

        contravariantInstance.Display(new Derived());  // Outputs "Derived"
    }
}
