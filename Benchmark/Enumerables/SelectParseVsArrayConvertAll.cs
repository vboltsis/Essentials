using System.Reflection;
using System.Runtime.CompilerServices;

namespace Benchmark.Classes;

/*
|                 Method |     Mean |   Error |  StdDev | Rank |   Gen0 | Allocated |
|----------------------- |---------:|--------:|--------:|-----:|-------:|----------:|
|         GetParseValues | 171.7 ns | 1.52 ns | 1.42 ns |    3 | 0.0114 |     144 B |
|        GetParseValues2 | 180.3 ns | 3.62 ns | 4.03 ns |    4 | 0.0114 |     144 B |
|  GetArrayConvertValues | 138.7 ns | 0.93 ns | 0.83 ns |    1 | 0.0076 |      96 B |
| GetArrayConvertValues2 | 143.7 ns | 1.23 ns | 1.15 ns |    2 | 0.0076 |      96 B |
 */

[RankColumn]
[MemoryDiagnoser]
public class SelectParseVsArrayConvertAll
{
    private static RedisValueCopy[] _values = new RedisValueCopy[]
    {
        new RedisValueCopy("1"),
        new RedisValueCopy("11"),
        new RedisValueCopy("31"),
        new RedisValueCopy("12"),
        new RedisValueCopy("13"),
        new RedisValueCopy("13"),
        new RedisValueCopy("13"),
        new RedisValueCopy("12"),
        new RedisValueCopy("551"),
        new RedisValueCopy("13"),
        new RedisValueCopy("13"),
        new RedisValueCopy("188"),
        new RedisValueCopy("81"),
        new RedisValueCopy("91"),
        new RedisValueCopy("14"),
        new RedisValueCopy("145"),
        new RedisValueCopy("122"),
        new RedisValueCopy("1223"),
    };

    [Benchmark]
    public int[] GetParseValues()
    {
        return _values.Select(x => int.Parse(x.Value)).ToArray();
    }

    [Benchmark]
    public int[] GetParseValues2()
    {
        return _values.Select(x => Convert.ToInt32(x.Value)).ToArray();
    }

    [Benchmark]
    public int[] GetArrayConvertValues()
    {
        return Array.ConvertAll(_values, x => int.Parse(x.Value));
    }

    [Benchmark]
    public int[] GetArrayConvertValues2()
    {
        return Array.ConvertAll(_values, x => Convert.ToInt32(x.Value));
    }
}


public readonly struct RedisValueCopy
{
    public string Value { get; }

    internal enum StorageType
    {
        Null,
        Int64,
        UInt64,
        Double,
        Raw,
        String
    }

    internal static readonly RedisValueCopy[] EmptyArray = Array.Empty<RedisValueCopy>();

    private readonly object _objectOrSentinel;

    private readonly ReadOnlyMemory<byte> _memory;

    private readonly long _overlappedBits64;

    private static readonly object Sentinel_SignedInteger = new object();

    private static readonly object Sentinel_UnsignedInteger = new object();

    private static readonly object Sentinel_Raw = new object();

    private static readonly object Sentinel_Double = new object();

    private static readonly object s_DoubleNAN = double.NaN;

    private static readonly object s_DoublePosInf = double.PositiveInfinity;

    private static readonly object s_DoubleNegInf = double.NegativeInfinity;

    private static readonly object s_EmptyString = EmptyString;

    private static readonly object[] s_CommonInt32 = Enumerable.Range(-1, 22).Select((Func<int, object>)((int i) => i)).ToArray();

    private static readonly FieldInfo s_origin = typeof(MemoryStream).GetField("_origin", BindingFlags.Instance | BindingFlags.NonPublic);

    private static readonly FieldInfo s_buffer = typeof(MemoryStream).GetField("_buffer", BindingFlags.Instance | BindingFlags.NonPublic);

    internal object DirectObject => _objectOrSentinel;

    internal long DirectOverlappedBits64 => _overlappedBits64;

    //
    // Summary:
    //     Represents the string ""
    public static RedisValueCopy EmptyString { get; } = new RedisValueCopy(0L, default(ReadOnlyMemory<byte>), Sentinel_Raw);


    //
    // Summary:
    //     A null value
    public static RedisValueCopy Null { get; } = new RedisValueCopy(0L, default(ReadOnlyMemory<byte>), null);


    //
    // Summary:
    //     Indicates whether the value is a primitive integer (signed or unsigned)
    public bool IsInteger
    {
        get
        {
            if (_objectOrSentinel != Sentinel_SignedInteger)
            {
                return _objectOrSentinel == Sentinel_UnsignedInteger;
            }

            return true;
        }
    }

    //
    // Summary:
    //     Indicates whether the value should be considered a null value
    public bool IsNull => _objectOrSentinel == null;

    //
    // Summary:
    //     Indicates whether the value is either null or a zero-length value
    public bool IsNullOrEmpty
    {
        get
        {
            if (IsNull)
            {
                return true;
            }

            if (_objectOrSentinel == Sentinel_Raw && _memory.IsEmpty)
            {
                return true;
            }

            string text = _objectOrSentinel as string;
            if (text != null && text.Length == 0)
            {
                return true;
            }

            byte[] array = _objectOrSentinel as byte[];
            if (array != null && array.Length == 0)
            {
                return true;
            }

            return false;
        }
    }

    //
    // Summary:
    //     Indicates whether the value is greater than zero-length or has an integer value
    public bool HasValue => !IsNullOrEmpty;

    private double OverlappedValueDouble
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return BitConverter.Int64BitsToDouble(_overlappedBits64);
        }
    }

    internal long OverlappedValueInt64
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return _overlappedBits64;
        }
    }

    internal ulong OverlappedValueUInt64
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return (ulong)_overlappedBits64;
        }
    }

    internal StorageType Type
    {
        get
        {
            object objectOrSentinel = _objectOrSentinel;
            if (objectOrSentinel == null)
            {
                return StorageType.Null;
            }

            if (objectOrSentinel == Sentinel_SignedInteger)
            {
                return StorageType.Int64;
            }

            if (objectOrSentinel == Sentinel_Double)
            {
                return StorageType.Double;
            }

            if (objectOrSentinel == Sentinel_Raw)
            {
                return StorageType.Raw;
            }

            if (objectOrSentinel is string)
            {
                return StorageType.String;
            }

            if (objectOrSentinel is byte[])
            {
                return StorageType.Raw;
            }

            if (objectOrSentinel == Sentinel_UnsignedInteger)
            {
                return StorageType.UInt64;
            }

            throw new InvalidOperationException("Unknown type");
        }
    }

    private RedisValueCopy(long overlappedValue64, ReadOnlyMemory<byte> memory, object objectOrSentinel)
    {
        _overlappedBits64 = overlappedValue64;
        _memory = memory;
        _objectOrSentinel = objectOrSentinel;
    }

    internal RedisValueCopy(object obj, long overlappedBits)
    {
        _overlappedBits64 = overlappedBits;
        _objectOrSentinel = obj;
        _memory = default(ReadOnlyMemory<byte>);
    }

    //
    // Summary:
    //     Creates a StackExchange.Redis.RedisValue from a string.
    public RedisValueCopy()
        : this(0L, default(ReadOnlyMemory<byte>), "")
    {
    }

    public RedisValueCopy(string value)
    {
        Value = value;
    }
}