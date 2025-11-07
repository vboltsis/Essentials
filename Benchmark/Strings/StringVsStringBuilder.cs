using Microsoft.Extensions.ObjectPool;
using System.Text;

namespace Benchmark;
/*
| Method                        | _number | Mean         | Error      | StdDev     | Ratio | Gen0   | Allocated | Alloc Ratio |
|------------------------------ |-------- |-------------:|-----------:|-----------:|------:|-------:|----------:|------------:|
| LogFileOfferString            | 1       |    401.49 ns |   7.006 ns |   6.211 ns |  1.00 | 0.0720 |     904 B |        1.00 |
| LogFileOffer                  | 1       |     51.39 ns |   0.934 ns |   0.959 ns |  0.13 | 0.0223 |     280 B |        0.31 |
| LogFileOfferStringBuilderPool | 1       |     36.28 ns |   0.296 ns |   0.277 ns |  0.09 | 0.0057 |      72 B |        0.08 |
|                               |         |              |            |            |       |        |           |             |
| LogFileOfferString            | 10      |  2,621.33 ns |  24.822 ns |  22.004 ns |  1.00 | 0.4005 |    5064 B |        1.00 |
| LogFileOffer                  | 10      |    288.06 ns |   5.811 ns |   7.136 ns |  0.11 | 0.0591 |     744 B |        0.15 |
| LogFileOfferStringBuilderPool | 10      |    232.39 ns |   1.111 ns |   1.040 ns |  0.09 | 0.0157 |     200 B |        0.04 |
|                               |         |              |            |            |       |        |           |             |
| LogFileOfferString            | 100     | 25,509.03 ns | 285.794 ns | 267.332 ns |  1.00 | 3.8757 |   48801 B |        1.00 |
| LogFileOffer                  | 100     |  2,195.57 ns |  17.243 ns |  16.935 ns |  0.09 | 0.3242 |    4104 B |        0.08 |
| LogFileOfferStringBuilderPool | 100     |  1,998.10 ns |  19.598 ns |  16.365 ns |  0.08 | 0.1221 |    1552 B |        0.03 |
*/

[MemoryDiagnoser] 
public class StringVsStringBuilder
{
    [Params(1, 10, 100)]
    public int Number;
    private List<FileManager> Files { get; set; }

    private static readonly ObjectPool<StringBuilder> StringBuilderPool =
        new DefaultObjectPool<StringBuilder>(new StringBuilderPooledObjectPolicy
        {
            InitialCapacity = 256,
        }, 100);


    [GlobalSetup]
    public void GlobalSetup()
    {
        Files = GetFileManagers(Number);
    }

    [Benchmark(Baseline = true)]
    public string LogFileOfferString()
    {
        var logMessage = $"File Offers{Environment.NewLine}" +
                         string.Join(Environment.NewLine,
                         Files.Select(c => FileResponseLogFormat(c, c.FileError.FileErrorMetadata.FileAmount, c.FileError)));

        return logMessage;
    }

    private string FileResponseLogFormat(FileManager file, decimal? fileAmount, FileError fileError)
    {
        return $"{file.Id}: {(fileAmount.HasValue ? $"{fileAmount}" : fileError?.ToString())}";
    }

    [Benchmark]
    public string LogFileOffer()
    {
        var separator = ": ";
        var builder = new StringBuilder();
        builder.AppendLine("File Offers");
        foreach (var file in Files)
        {
            builder.Append(file.Id);
            builder.Append(separator);
            if (file.FileError?.FileErrorMetadata?.FileAmount.HasValue == true)
            {
                builder.Append(file.FileError.FileErrorMetadata.FileAmount.Value);
            }
            else
            {
                builder.Append(file.FileError?.FileErrorMetadata?.Error);
            }
            builder.Append(Environment.NewLine);
        }

        return builder.ToString();
    }

    [Benchmark]
    public string LogFileOfferStringBuilderPool()
    {
        var separator = ": ";
        var builder = StringBuilderPool.Get();
        builder.AppendLine("File Offers");
        foreach (var file in Files)
        {
            builder.Append(file.Id);
            builder.Append(separator);
            if (file.FileError?.FileErrorMetadata?.FileAmount.HasValue == true)
            {
                builder.Append(file.FileError.FileErrorMetadata.FileAmount.Value);
            }
            else
            {
                builder.Append(file.FileError?.FileErrorMetadata?.Error);
            }
            builder.Append(Environment.NewLine);
        }

        var result = builder.ToString();
        builder.Clear();
        StringBuilderPool.Return(builder);
        return result;
    }

    private static List<FileManager> GetFileManagers(int amount)
    {
        var list = new List<FileManager>(amount);
        for (int i = 0; i < amount; i++)
        {
            list.Add(new FileManager
            {
                Id = 1,
                Name = "File Name",
                FileError = new FileError
                {
                    Code = FileErrorCode.Error1,
                    FileErrorMetadata = new FileErrorMetadata
                    {
                        Error = "this",
                        FileAmount = i % 2 == 0 ? null : 2,
                        MinimumAllowedFiles = 4,
                        EventName = "Event Name",
                        Number = 420,
                        RandomId = 69
                    }
                }
            });
        }

        return list;
    }
}

public class FileManager
{
    public int Id { get; set; }
    public string Name { get; set; }
    public FileError FileError{ get; set; }
}

public class FileError
{
    public FileErrorCode Code { get; set; }

    public FileErrorMetadata FileErrorMetadata { get; set; }

    public override string ToString() =>
        Code +
        (FileErrorMetadata != null
            ? $" {FileErrorMetadata.Error} ({FileErrorMetadata.RandomId}) - {FileErrorMetadata.EventName}. " +
              $" Amount: {FileErrorMetadata.FileAmount}, MinAllowed: {FileErrorMetadata.MinimumAllowedFiles}, Odds: {FileErrorMetadata.Number}"
            : "");
}

public enum FileErrorCode
{
    Error1 = 0,
    Error2 = 1
}

public class FileErrorMetadata
{
    public decimal? MinimumAllowedFiles { get; set; }

    public decimal? FileAmount { get; set; }

    public long? RandomId { get; set; }

    public string Error { get; set; }

    public string EventName { get; set; }

    public decimal? Number { get; set; }
}