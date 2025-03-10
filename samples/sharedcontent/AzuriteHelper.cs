using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace samples;

public static class AzuriteHelper
{
    public static void Start()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            Process.Start($"{Environment.CurrentDirectory}\\run-azurite.bat");
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            Process.Start($"{Environment.CurrentDirectory}\\run-azurite.sh");
        Thread.Sleep(4000);
    }
}
