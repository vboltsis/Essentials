using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics;

public class MemoryMetricsService : IMemoryMetricsService
{
    public IRuntimeInformationService RuntimeInformationService { get; set; }
    public MemoryMetricsService(IRuntimeInformationService runtimeInformationService)
    {
        RuntimeInformationService = runtimeInformationService;
    }

    public MemoryMetrics GetMetrics()
    {
        if (RuntimeInformationService.IsUnix())
        {
            return GetUnixMetrics();
        }

        return GetWindowsMetrics();
    }

    private MemoryMetrics GetWindowsMetrics()
    {
        var output = "";

        var info = new ProcessStartInfo();
        info.FileName = "wmic";
        info.Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value";
        info.RedirectStandardOutput = true;

        using (var process = Process.Start(info))
        {
            output = process.StandardOutput.ReadToEnd();
        }

        var lines = output.Trim().Split("\n");
        var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
        var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);

        var metrics = new MemoryMetrics(Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0),
            Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0));

        return metrics;
    }

    private MemoryMetrics GetUnixMetrics()
    {
        var output = "";

        var info = new ProcessStartInfo("free -m");
        info.FileName = "/bin/bash";
        info.Arguments = "-c \"free -m\"";
        info.RedirectStandardOutput = true;

        using (var process = Process.Start(info))
        {
            output = process.StandardOutput.ReadToEnd();
            Console.WriteLine(output);
        }

        var lines = output.Split("\n");
        var memory = lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);

        var metrics = new MemoryMetrics(double.Parse(memory[1]), double.Parse(memory[2]));

        return metrics;
    }
}
