using ProtoBuf;

namespace FeatureExamples;

internal class ProtobufExample
{
    public static void Example()
    {
        var customer = new Customer { Id = 1, Name = "John Doe", Email = "john.doe@example.com" };

        using (FileStream stream = new FileStream("customer.bin", FileMode.Create))
        {
            Serializer.Serialize(stream, customer);
        }

        using (FileStream stream = new FileStream("customer.bin", FileMode.Open))
        {
            Customer deserializedCustomer = Serializer.Deserialize<Customer>(stream);
            Console.WriteLine($"Deserialized Customer: {deserializedCustomer.Name}, {deserializedCustomer.Email}");
        }
    }
}

[ProtoContract]
public class Customer
{
    [ProtoMember(1)]
    public int Id { get; set; }

    [ProtoMember(2)]
    public string Name { get; set; }

    [ProtoMember(3)]
    public string Email { get; set; }
}