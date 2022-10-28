namespace AllDotNetInterfaces;

//IEquatable tests whether two objects are equal.
public class Customer : IEquatable<Customer>
{
    public int Id { get; set; }
    public string Name { get; set; }

    public bool Equals(Customer other)
    {
        return Name == other.Name && Id == other.Id;
    }
}

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
}
