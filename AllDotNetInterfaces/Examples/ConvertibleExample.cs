namespace AllDotNetInterfaces;

public class Temperature : IConvertible
{
    public double Celsius { get; set; }

    public TypeCode GetTypeCode()
    {
        throw new NotImplementedException();
    }

    public bool ToBoolean(IFormatProvider provider)
    {
        return Celsius < 0; // true if below freezing point
    }

    public byte ToByte(IFormatProvider provider)
    {
        throw new InvalidCastException("Cannot cast Temperature to Byte.");
    }

    public char ToChar(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public DateTime ToDateTime(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public decimal ToDecimal(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public double ToDouble(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public short ToInt16(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public int ToInt32(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public long ToInt64(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public sbyte ToSByte(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public float ToSingle(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    // ... (similar methods for other conversions)

    public string ToString(IFormatProvider provider)
    {
        return $"{Celsius}°C";
    }

    public object ToType(Type conversionType, IFormatProvider provider)
    {
        if (conversionType == typeof(string)) return ToString(provider);
        throw new InvalidCastException($"Cannot cast Temperature to {conversionType.Name}.");
    }

    public ushort ToUInt16(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public uint ToUInt32(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }

    public ulong ToUInt64(IFormatProvider provider)
    {
        throw new NotImplementedException();
    }
}
