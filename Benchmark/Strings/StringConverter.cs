using System.Buffers;
using System.Buffers.Text;
using System.Runtime.InteropServices;

namespace Benchmark.Classes;
/*
|                          Method |     Mean |    Error |   StdDev | Ratio | Allocated | Alloc Ratio |
|-------------------------------- |---------:|---------:|---------:|------:|----------:|------------:|
| ConvertGuidToBase64StringFaster | 30.43 ns | 0.372 ns | 0.311 ns |  0.49 |      72 B |        0.39 |
|   ConvertGuidToBase64StringFast | 34.27 ns | 0.726 ns | 0.713 ns |  0.55 |      72 B |        0.39 |
|       ConvertGuidToBase64String | 61.97 ns | 1.197 ns | 0.935 ns |  1.00 |     184 B |        1.00 |

BenchmarkRunner.Run<StringConverterBenchmark>();
BenchmarkRunner.Run<SpanBenchmark>();
BenchmarkRunner.Run<LogFileBenchmark>();
*/
//Add R’s \bin\ directory to the PATH system environment variable Example: variable name: R_HOME, value: C:\Program Files\R\R-4.2.2
[RPlotExporter]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[MemoryDiagnoser(displayGenColumns:false)]
public class StringConverter
{
    private const byte ForwardSlashByte = (byte)'/';
    private const byte PlusByte = (byte)'+';
    private const char Underscore = '_';
    private const char Dash = '-';
    private static Guid _input = Guid.NewGuid();

    [Benchmark(Baseline = true)]
    public string ConvertGuidToBase64String()
    {
        return Convert.ToBase64String(_input.ToByteArray())
            .Replace("/", "-")
            .Replace("+", "_")
            .Replace("=", "");
    }

    [Benchmark]
    public string ConvertGuidToBase64StringFast()
    {
        var buffer = ArrayPool<byte>.Shared.Rent(16);
        _input.TryWriteBytes(buffer);

        var base64string = Convert.ToBase64String(buffer);

        ArrayPool<byte>.Shared.Return(buffer);

        return base64string;
    }

    [Benchmark]
    public string ConvertGuidToBase64StringFaster()
    {
        Span<byte> guidBytes = stackalloc byte[16];
        Span<byte> encodedBytes = stackalloc byte[24];

        MemoryMarshal.TryWrite(guidBytes, ref _input);
        Base64.EncodeToUtf8(guidBytes, encodedBytes, out _, out _);

        Span<char> chars = stackalloc char[22];

        for (var i = 0; i < 22; i++)
        {
            chars[i] = encodedBytes[i] switch
            {
                ForwardSlashByte => Dash,
                PlusByte => Underscore,
                _ => (char)encodedBytes[i],
            };
        }

        var final = new string(chars);

        return final;
    }
}
