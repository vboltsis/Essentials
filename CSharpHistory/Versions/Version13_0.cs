using System.Runtime.CompilerServices;

namespace CSharpHistory;

public class Version13_0
{
    public void PrintNames<T>(params IEnumerable<T> names)
    {
        foreach (var name in names)
        {
            Console.WriteLine(name);
        }
    }

    [OverloadResolutionPriority(1)]
    public void PrintNames<T>(params ReadOnlySpan<T> names)
    {
        foreach (var name in names)
        {
            Console.WriteLine(name);
        }
    }
}

class Numbers
{
    public int[] Values { get; set; } = new int[10];
    public void IndexFromEnd()
    {
        var x = new Numbers
        {
            Values =
            {
                [1] = 420,
                [^1] = 69
            }
        };
    }
}

ref struct MyRefStruct : IComparable<MyRefStruct>
{
    private int _value;
    public int CompareTo(MyRefStruct other) => _value.CompareTo(other._value);
}


//ref partial struct MyFile
//{
//    public partial Span<byte> Map { get; private set; }
//    public partial void Dispose();
//}

//ref partial struct MyFile
//{
//    Span<byte> _map;
//    Lock mapLock = new();

//    public partial Span<byte> Map
//    {
//        get { lock (mapLock) { return _map; } }
//        private set { lock (maplock) { _map = value; } }
//    }

//    public partial void Dispose() => Map = default;
//}
