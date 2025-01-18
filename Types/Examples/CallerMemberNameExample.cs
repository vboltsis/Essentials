using System.Runtime.CompilerServices;

namespace FeatureExamples;

public class CallerMemberNameExample
{
    public void ExampleMethod([CallerMemberName] string? memberName = null)
    {
        Console.WriteLine(memberName);
    }
}
