using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeatureExamples;

public class PatternMatchingExamples
{
    public static void SwitchExample()
    {
        var person = new Person("John", "Doe", 10);

        var greeting = person switch
        {
            ("John", "Doe", 10) => "Hello, John Doe!",
            ("Jane", "Doe", 15) => "Hello, Jane Doe!",
            _ => "Hello, stranger!"
        };

        Console.WriteLine(greeting);
    }

    public static void PropertyExample()
    {
        Person person = null;

        // if (person != null && person.FirstName == "Test" && person.Age > 18)
        // {
        //     
        // }

        if (person is {FirstName: "test", Age: > 18})
        {
            
        }
    }
}

public record Person(string FirstName, string LastName, int Age);
