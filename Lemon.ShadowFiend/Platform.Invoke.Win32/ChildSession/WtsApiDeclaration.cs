using System.Runtime.InteropServices;

namespace Platform.Invoke.Win32.ChildSession;

public static class WtsApiDeclaration
{
    [DllImport("Wtsapi32.dll", SetLastError = true)]
    public static extern bool WTSEnableChildSessions(bool enable);

    [DllImport("Wtsapi32.dll", SetLastError = true)]
    public static extern bool WTSIsChildSessionsEnabled(out bool isEnabled);

    [DllImport("Wtsapi32.dll", SetLastError = true)]
    public static extern bool WTSGetChildSessionId(out uint sessionId);
    
    [DllImport("Wtsapi32.dll", SetLastError = true)]
    public static extern bool WTSLogoffSession(nint hServer, uint sessionId, bool bWait);

    [DllImport("Wtsapi32.dll", SetLastError = true)]
    public static extern bool WTSDisconnectSession(nint hServer, uint sessionId, bool bWait);
}