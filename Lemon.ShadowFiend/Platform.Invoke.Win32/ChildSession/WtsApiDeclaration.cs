using System.Runtime.InteropServices;

namespace Platform.Invoke.Win32.ChildSession;

public static partial class WtsApiDeclaration
{
    [LibraryImport("Wtsapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WTSEnableChildSessions([MarshalAs(UnmanagedType.Bool)] bool enable);

    [LibraryImport("Wtsapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WTSIsChildSessionsEnabled([MarshalAs(UnmanagedType.Bool)] out bool isEnabled);

    [LibraryImport("Wtsapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WTSGetChildSessionId(out uint sessionId);

    [LibraryImport("Wtsapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WTSLogoffSession(nint hServer, uint sessionId, [MarshalAs(UnmanagedType.Bool)] bool bWait);

    [LibraryImport("Wtsapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool WTSDisconnectSession(nint hServer, uint sessionId, [MarshalAs(UnmanagedType.Bool)] bool bWait);
}