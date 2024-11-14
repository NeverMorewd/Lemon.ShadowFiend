using System.Runtime.InteropServices;

namespace Platform.Invoke.Win32.OS;

public static class OSHelper
{
    public static bool IsSupportedChildSession()
    {
        return IsSupportedChildSessionCore();
    }
    private static bool IsSupportedChildSessionCore()
    {
        var platform = Environment.OSVersion.Platform;
        var curOsVersion = Environment.OSVersion.Version;
        if (platform == PlatformID.Win32NT && curOsVersion >= new Version(6, 2, 9200, 0))
        {
            return true;
        }
        return IsOS(OS.OS_ANYSERVER) && (curOsVersion.Major > 6 || curOsVersion is { Major: 6, Minor: >= 2 });
    }
    [DllImport("ShlwApi.dll", CharSet = CharSet.Auto)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    private static extern bool IsOS(OS os);

    private enum OS : uint
    {
        OS_WINDOWS = 0u,
        OS_NT = 1u,
        OS_WIN95ORGREATER = 2u,
        OS_NT4ORGREATER = 3u,
        OS_WIN98ORGREATER = 5u,
        OS_WIN98_GOLD = 6u,
        OS_WIN2000ORGREATER = 7u,
        OS_WIN2000PRO = 8u,
        OS_WIN2000SERVER = 9u,
        OS_WIN2000ADVSERVER = 10u,
        OS_WIN2000DATACENTER = 11u,
        OS_WIN2000TERMINAL = 12u,
        OS_EMBEDDED = 13u,
        OS_TERMINALCLIENT = 14u,
        OS_TERMINALREMOTEADMIN = 15u,
        OS_WIN95_GOLD = 16u,
        OS_MEORGREATER = 17u,
        OS_XPORGREATER = 18u,
        OS_HOME = 19u,
        OS_PROFESSIONAL = 20u,
        OS_DATACENTER = 21u,
        OS_ADVSERVER = 22u,
        OS_SERVER = 23u,
        OS_TERMINALSERVER = 24u,
        OS_PERSONALTERMINALSERVER = 25u,
        OS_FASTUSERSWITCHING = 26u,
        OS_WELCOMELOGONUI = 27u,
        OS_DOMAINMEMBER = 28u,
        OS_ANYSERVER = 29u,
        OS_WOW6432 = 30u,
        OS_WEBSERVER = 31u,
        OS_SMALLBUSINESSSERVER = 32u,
        OS_TABLETPC = 33u,
        OS_SERVERADMINUI = 34u,
        OS_MEDIACENTER = 35u,
        OS_APPLIANCE = 36u
    }
}