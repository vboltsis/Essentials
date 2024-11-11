using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureExamples;

public class GoToExample
{
    public void ProcessData()
    {
        var a = 1;
        if (a == 1)
            goto Error;

        PerformSomeAction();
        return;

    Error:
        LogError();
    }

    private void LogError()
    {
        Console.WriteLine("Error");
    }

    private void PerformSomeAction()
    {
        Console.WriteLine("hello");
    }
}
