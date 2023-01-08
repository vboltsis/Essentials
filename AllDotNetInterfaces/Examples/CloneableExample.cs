namespace AllDotNetInterfaces;

public class Consumer : ICloneable
{
    public string Name { get; set; }
    public int Age { get; set; }
    public Address Address { get; set; }

    public object Clone()
    {
        return new Consumer
        {
            Name = Name,
            Age = Age,
            Address = new Address
            {
                Street = Address.Street,
                City = Address.City,
                State = Address.State,
                Zip = Address.Zip
            }
        };
    }
}

public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
}
