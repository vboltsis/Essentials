using System.Text;

namespace Benchmark.Classes;
/*
|             Method |   N |         Mean |      Error |     StdDev | Ratio |   Gen0 |   Gen1 | Allocated | Alloc Ratio |
|------------------- |---- |-------------:|-----------:|-----------:|------:|-------:|-------:|----------:|------------:|
| LogFileOfferString |   1 |    354.28 ns |   2.496 ns |   2.213 ns |  1.00 | 0.0720 |      - |     904 B |        1.00 |
|       LogFileOffer |   1 |     46.39 ns |   0.489 ns |   0.544 ns |  0.13 | 0.0223 |      - |     280 B |        0.31 |
|                    |     |              |            |            |       |        |        |           |             |
| LogFileOfferString |  10 |  2,392.61 ns |  21.128 ns |  17.643 ns |  1.00 | 0.4005 |      - |    5064 B |        1.00 |
|       LogFileOffer |  10 |    251.85 ns |   2.698 ns |   2.391 ns |  0.11 | 0.0591 |      - |     744 B |        0.15 |
|                    |     |              |            |            |       |        |        |           |             |
| LogFileOfferString | 100 | 23,330.02 ns | 305.532 ns | 428.314 ns |  1.00 | 3.8757 | 0.0916 |   48801 B |        1.00 |
|       LogFileOffer | 100 |  2,028.05 ns |  18.789 ns |  16.656 ns |  0.09 | 0.3242 | 0.0038 |    4104 B |        0.08 |
 */

[MemoryDiagnoser]
public class StringVsStringBuilder
{
    [Params(1, 10, 100)]
    public int _number;
    private List<FileManager> _files { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _files = GetFileManagers(_number);
    }

    [Benchmark(Baseline = true)]
    public string LogFileOfferString()
    {
        var logMessage = $"File Offers{Environment.NewLine}" +
                         string.Join(Environment.NewLine,
                         _files.Select(c => FileResponseLogFormat(c, c.FileError.FileErrorMetadata.FileAmount, c.FileError)));

        return logMessage;
    }

    private string FileResponseLogFormat(FileManager file, decimal? FileAmount, FileError FileError)
    {
        return $"{file.Id}: {(FileAmount.HasValue ? $"{FileAmount}" : FileError?.ToString())}";
    }

    [Benchmark]
    public string LogFileOffer()
    {
        var separator = ": ";
        var builder = new StringBuilder();
        builder.AppendLine("File Offers");
        foreach (var file in _files)
        {
            builder.Append(file.Id);
            builder.Append(separator);
            if (file.FileError.FileErrorMetadata?.FileAmount.HasValue == true)
            {
                builder.Append(file.FileError.FileErrorMetadata.FileAmount.Value);
            }
            else
            {
                builder.Append(file.FileError.FileErrorMetadata.Error);
            }
            builder.Append(Environment.NewLine);
        }

        return builder.ToString();
    }

    private static List<FileManager> GetFileManagers(int amount)
    {
        var list = new List<FileManager>();
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

    public FileError Clone() => new()
    {
        FileErrorMetadata = FileErrorMetadata?.Clone(),
        Code = Code
    };

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

    public FileErrorMetadata Clone() =>
        new()
        {
            RandomId = RandomId,
            Error = Error,
            EventName = EventName,
            FileAmount = FileAmount,
            MinimumAllowedFiles = MinimumAllowedFiles,
            Number = Number
        };
}