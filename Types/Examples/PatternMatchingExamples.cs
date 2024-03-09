using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureExamples;

public class PatternMatchingExamples
{
    public static void Example()
    {
        var person = new Person("John", "Doe");

        var greeting = person switch
        {
            ("John", "Doe") => "Hello, John Doe!",
            ("Jane", "Doe") => "Hello, Jane Doe!",
            _ => "Hello, stranger!"
        };

        Console.WriteLine(greeting);
    }
}

public record Person(string FirstName, string LastName);
