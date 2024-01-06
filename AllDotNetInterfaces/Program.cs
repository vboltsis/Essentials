using AllDotNetInterfaces;

//1. IComparable Example
//var person1 = new Person
//{
//    Id = 1,
//    Name = "John",
//    Address = "Williams 24"
//};

//var person2 = new Person
//{
//    Id = 2,
//    Name = "Aaron",
//    Address = "Baron 22"
//};

//var person3 = new Person
//{
//    Id = 3,
//    Name = "Bob",
//    Address = "Arena 4"
//};

//var people = new Person[] { person1, person2, person3 };
//Array.Sort(people);
//Console.WriteLine(people[0].Name);

//2. IEquatable Example
//var customer = new Customer
//{
//    Id = 1,
//    Name = "Test"
//};

//var customer2 = new Customer
//{
//    Id = 1,
//    Name = "Test"
//};

//Console.WriteLine(customer == customer2);
//Console.WriteLine(customer.Equals(customer2));
//Console.WriteLine();

//var student = new Student
//{
//    Id = 1,
//    Name = "Test"
//};

//var student2 = new Student
//{
//    Id = 1,
//    Name = "Test"
//};

//Console.WriteLine("STUDENTS");
//Console.WriteLine(student == student2);
//Console.WriteLine(student.Equals(student2));

//3. ICloneable Example
//var consumer = new Consumer
//{
//    Name = "John",
//    Age = 30,
//    Address = new Address
//    {
//        Street = "123 Main St",
//        City = "New York",
//        State = "NY",
//        Zip = "10001"
//    }
//};

//Consumer clonedConsumer = (Consumer)consumer.Clone();

//Console.WriteLine(clonedConsumer.Name);       // Output: "John"
//Console.WriteLine(clonedConsumer.Age);        // Output: 30
//Console.WriteLine(clonedConsumer.Address.City); // Output: "New York"
//Console.WriteLine(consumer == clonedConsumer); //Output: false

//4. ISpanFormattable Example
//SpanFormattableExample.PrintHuman();
//Console.WriteLine();