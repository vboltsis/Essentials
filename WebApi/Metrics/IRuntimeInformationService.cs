using System.Runtime.InteropServices;

public interface IRuntimeInformationService
{
    bool IsUnix();
    OSPlatform GetOsPlatform();
    string GetFrameworkDescription();
    string GetOSDescription();
    Architecture GetOSArchitecture();
    string GetRuntimeIdentifier();
    bool IsOSX();
    bool IsFreeBSD();
    bool IsLinux();
    bool IsWindows();
}