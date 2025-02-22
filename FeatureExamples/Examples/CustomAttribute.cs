using System.Reflection;

namespace FeatureExamples;

[Author("John Doe", "1.0")]
public class SampleClass
{
    [Author("John Doe", "1.1")]
    public void SampleMethod()
    {
        Console.WriteLine("This is a sample method.");
    }

    public static void Example()
    {
        Type type = typeof(SampleClass);

        // Get the Author attribute on the class
        AuthorAttribute classAttribute = (AuthorAttribute)Attribute.GetCustomAttribute(type, typeof(AuthorAttribute));
        if (classAttribute != null)
        {
            Console.WriteLine($"Class {type.Name} authored by {classAttribute.Name}, version {classAttribute.Version}");
        }

        // Get the Author attribute on the method
        MethodInfo method = type.GetMethod("SampleMethod");
        AuthorAttribute methodAttribute = (AuthorAttribute)Attribute.GetCustomAttribute(method, typeof(AuthorAttribute));
        if (methodAttribute != null)
        {
            Console.WriteLine($"Method {method.Name} authored by {methodAttribute.Name}, version {methodAttribute.Version}");
        }
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
public class AuthorAttribute : Attribute
{
    public string Name { get; }
    public string Version { get; }

    public AuthorAttribute(string name, string version)
    {
        Name = name;
        Version = version;
    }
}
