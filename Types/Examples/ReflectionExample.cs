using System.Reflection;

namespace FeatureExamples;

public class ReflectionExample
{
    public static void GetMethods<T>()
    {
        var methods = typeof(T).GetMethods();

        foreach (var method in methods)
        {
            Console.WriteLine(method.Name);
        }
    }

    public static void GetProperties<T>()
    {
        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            Console.WriteLine(property.Name);
        }
    }

    public void MyMethod()
    {
        Console.WriteLine("Hello from MyMethod");
    }

    public static void InvokeMyMethod<T>()
    {
        var method = typeof(T).GetMethod("MyMethod");
        var instance = Activator.CreateInstance<T>();
        method.Invoke(instance, null);
    }

    public static void AssemblyTypes()
    {
        var assembly = Assembly.GetExecutingAssembly();
        Console.WriteLine($"Assembly Full Name: {assembly.FullName}");

        Type[] types = assembly.GetTypes();
        foreach (Type type in types)
        {
            Console.WriteLine($"Type: {type.FullName}");
        }
    }
}
