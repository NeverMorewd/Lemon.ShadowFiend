using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Lemon.ShadowFiend.Utils;

public static class UtilExtensions
{
    public static SecureString ToSecureString(this string input)
    {
        var secureString = new SecureString();
        foreach (var c in input)
        {
            secureString.AppendChar(c);
        }
        secureString.MakeReadOnly();
        return secureString;
    }
    public static string? SecureStringToString(this SecureString secureString)
    {
        var unmanagedString = nint.Zero;
        try
        {
            unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
            return Marshal.PtrToStringUni(unmanagedString);
        }
        finally
        {
            if (unmanagedString != IntPtr.Zero)
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}