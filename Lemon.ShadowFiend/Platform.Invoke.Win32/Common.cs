using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Platform.Invoke.Win32;

public class Common
{
    public static string GetLastErrorMessage()
    {
        var code = Marshal.GetLastWin32Error();
        var win32Ex = new Win32Exception(code);
        return win32Ex.Message;
    }
    public static Win32Exception GetLastWin32Exception()
    {
        var code = Marshal.GetLastWin32Error();
        var win32Ex = new Win32Exception(code);
        return win32Ex;
    }
}