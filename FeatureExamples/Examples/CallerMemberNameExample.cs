using System.Runtime.CompilerServices;

namespace FeatureExamples;

public class CallerMemberNameExample
{
    public static void ExampleMethod([CallerMemberName] string? memberName = null)
    {
        Console.WriteLine(memberName);
    }
}
