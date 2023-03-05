using MessagePack;

namespace Benchmark.Classes;

[RankColumn]
[MemoryDiagnoser]
[DisassemblyDiagnoser]
public class Compressions
{
    private byte[] data;
    private MyClass[] deserializedData;

    [GlobalSetup]
    public void GlobalSetup() {
        // Generate some sample data to compress
        var sampleData = new MyClass[100_000];
        for (int i = 0; i < sampleData.Length; i++) {
            sampleData[i] = new MyClass {
                MyBool = true,
                MyDate = DateTime.Now.AddMinutes(1),
                MyInt = i,
                MyString = $"Hello World{i}"
            };
        }

        deserializedData = sampleData;
        var options = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray);
        // Serialize the data to a byte array
        data = MessagePackSerializer.Serialize(sampleData, options);
    }

    [Benchmark]
    public byte[] Compress() {
        return MessagePackSerializer.Serialize(deserializedData,
            MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray));
    }

    [Benchmark]
    public MyClass[] Decompress() {
        return MessagePackSerializer.Deserialize<MyClass[]>(data,
            MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4BlockArray));
    }
}

[MessagePackObject]
public class MyClass {

    [Key(0)]
    public int MyInt { get; set; }
    [Key(1)]
    public string MyString { get; set; }
    [Key(2)]
    public bool MyBool { get; set; }
    [Key(3)]
    public DateTime MyDate { get; set; }
}