namespace AllDotNetInterfaces;

//IComparable imposes a total ordering on the objects being compared.
public class Person : IComparable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }

    public int CompareTo(object obj)
    {
        var person = obj as Person;

        if (person == null)
        {
            return -1;
        }

        return Address.CompareTo(person.Address);
    }
}
