using System.Diagnostics;
using System.Runtime.InteropServices;
using Platform.Invoke.Win32.OS;

namespace Platform.Invoke.Win32.WtsApi;

public static class ChildSessionHelper
{
    public static bool IsSupportChildSession()
    {
        return OSHelper.IsSupportedChildSession();
    }

    public static bool DisableChildSession()
    {
        if (OSVersionHelper.IsSupportedOsVersion())
        {
            return Wtsapi32Invoker.WTSEnableChildSessions(false);
        }
        return false;
    }
    public static bool EnableChildSession()
    {
        if (OSVersionHelper.IsSupportedOsVersion())
        {
            return Wtsapi32Invoker.WTSEnableChildSessions(true);
        }
        return false;
    }
    public static bool IsEnableChildSession()
    {
        if (OSVersionHelper.IsSupportedOsVersion())
        {
            Wtsapi32Invoker.WTSIsChildSessionsEnabled(out var isEnabled);
            return isEnabled;
        }
        else
        {
            throw new InvalidOperationException("此操作系统不支持子会话!");
        }
    }
    public static bool LogOutChildSession()
    {
        unsafe
        {
            if (OSVersionHelper.IsSupportedOsVersion())
            {
                if (Wtsapi32Invoker.WTSGetChildSessionId(out uint SessionId) && !Wtsapi32Invoker.WTSLogoffSession((IntPtr)(void*)null, SessionId, bWait: true))
                {
                    Trace.TraceError($"ChildSession: Failed to logoff child session {SessionId}. Last error: {Marshal.GetLastWin32Error()}");
                    return false;
                }
            }
        }
        return true;
    }
    public static bool DisconnectChildSession()
    {
        if (OSVersionHelper.IsSupportedOsVersion())  
        {
            if (Wtsapi32Invoker.WTSGetChildSessionId(out uint SessionId) 
                && !Wtsapi32Invoker.WTSDisconnectSession(IntPtr.Zero, SessionId, bWait: true))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
}