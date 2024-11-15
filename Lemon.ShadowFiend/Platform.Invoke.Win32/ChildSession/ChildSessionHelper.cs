using Platform.Invoke.Win32.OS;
using System.Runtime.Versioning;

namespace Platform.Invoke.Win32.ChildSession;

[SupportedOSPlatform("windows")]
public static class ChildSessionHelper
{
    private const string UnSupportedTips = "Current OS version does not support Windows Child Session!";
    
    public static bool IsSupportChildSession()
    {
        return OSHelper.IsSupportedChildSession();
    }

    public static bool DisableChildSession()
    {
        if (!IsSupportChildSession())
        {
            throw new NotSupportedException(UnSupportedTips);
        }
        return WtsApiDeclaration.WTSEnableChildSessions(false);
    }

    public static bool EnableChildSession()
    {
        if (!IsSupportChildSession())
        {
            throw new NotSupportedException(UnSupportedTips);
        }
        return WtsApiDeclaration.WTSEnableChildSessions(true);
    }

    public static bool IsEnableChildSession()
    {
        if (!IsSupportChildSession())
        {
            throw new NotSupportedException(UnSupportedTips);
        }
        WtsApiDeclaration.WTSIsChildSessionsEnabled(out var isEnabled);
        return isEnabled;
    }

    public static bool LogOutChildSession()
    {
        if (!IsSupportChildSession())
        {
            throw new NotSupportedException(UnSupportedTips);
        }
        if (!WtsApiDeclaration.WTSGetChildSessionId(out var sessionId) ||
            WtsApiDeclaration.WTSLogoffSession((IntPtr)null, sessionId, bWait: true)) return true;
        return false;
    }
}