using System.Runtime.InteropServices;

namespace Platform.Invoke.Win32.Kernel32;

public class Kernel32Declaration
{
    [DllImport("advapi32.dll", SetLastError = true)]
    public static extern bool LogonUser(string lpszUsername,
                                        string lpszDomain,
                                        string lpszPassword,
                                        int dwLogonType,
                                        int dwLogonProvider,
                                        out IntPtr phToken);

    [DllImport("kernel32.dll")]
    public static extern int FormatMessage(int dwFlags, 
                                            ref IntPtr lpSource, 
                                            int dwMessageId, 
                                            int dwLanguageId,
                                            ref String lpBuffer, 
                                            int nSize, 
                                            ref IntPtr Arguments);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CloseHandle(IntPtr hObject);
}