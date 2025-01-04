using System.Diagnostics;

namespace FeatureExamples;

public class DebugExamples
{
    public void Example()
    {
        var value = 0;
        Debug.WriteLine("Hello World!", "Information");
        Debug.Assert(value > 0, "Hello World!", "Warning");

#if DEBUG
        Console.WriteLine("Hello World!", "Debug");
#endif
    }
}