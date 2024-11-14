using System.Diagnostics;
using System.Runtime.InteropServices;
using Platform.Invoke.Win32.OS;

namespace Platform.Invoke.Win32.ChildSession;

public static class ChildSessionHelper
{
    public static bool IsSupportChildSession()
    {
        return OSHelper.IsSupportedChildSession();
    }

    public static bool DisableChildSession()
    {
        if (!IsSupportChildSession())
        {
            throw new InvalidOperationException("Current OS version does not support Windows child session!");
        }
        return WtsApiDeclaration.WTSEnableChildSessions(false);
    }
    public static bool EnableChildSession()
    {
        if (!IsSupportChildSession())
        {
            throw new InvalidOperationException("Current OS version does not support Windows child session!");
        }
        var result = WtsApiDeclaration.WTSEnableChildSessions(true);
        if (!result)
        {
            
        }
        return result;
    }
    public static bool IsEnableChildSession()
    {
        if (!IsSupportChildSession())
        {
            throw new InvalidOperationException("Current OS version does not support Windows child session!");
        }
        WtsApiDeclaration.WTSIsChildSessionsEnabled(out var isEnabled);
        return isEnabled;
    }
    public static bool LogOutChildSession()
    {
        if (!IsSupportChildSession())
        {
            throw new InvalidOperationException("Current OS version does not support Windows child session!");
        }
        if (!WtsApiDeclaration.WTSGetChildSessionId(out var sessionId) ||
            WtsApiDeclaration.WTSLogoffSession((IntPtr)null, sessionId, bWait: true)) return true;
        Trace.TraceError($"ChildSession: Failed to logoff child session {sessionId}. Last error: {Marshal.GetLastWin32Error()}");
        return false;
    }
    public static bool DisconnectChildSession()
    {
        if (!IsSupportChildSession())
        {
            throw new InvalidOperationException("Current OS version does not support Windows child session!");
        }
        return WtsApiDeclaration.WTSGetChildSessionId(out var sessionId) 
               && !WtsApiDeclaration.WTSDisconnectSession(nint.Zero, sessionId, bWait: true);
    }
}