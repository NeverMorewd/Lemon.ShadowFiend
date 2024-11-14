using System;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading.Tasks;
using Akavache;
using Lemon.ShadowFiend.Models;
using Microsoft.Extensions.Logging;

namespace Lemon.ShadowFiend.Services;

public class WindowsIdentityService
{
    private readonly IBlobCache _cacheProvider;
    private readonly ILogger _logger;
    public WindowsIdentityService(IBlobCache cache, ILogger<WindowsIdentityService> logger)
    {
        _cacheProvider = cache;
        _logger = logger;
    }
    public async Task<string?> GetCurrentUserName()
    {
        var cuserName = await _cacheProvider.GetOrCreateObject(nameof(GetCurrentUserName), () =>
        {
            GetCurrentUserAndMachineNameCore(out var userName, out _);
            return userName;
        });
        return cuserName;
    }
    public async Task<string?> GetCurrentMachineName()
    {
        var cmachineName = await _cacheProvider.GetOrCreateObject(nameof(GetCurrentMachineName), () =>
        {
            GetCurrentUserAndMachineNameCore(out _, out var machineName);
            return machineName;
        });
        return cmachineName;
    }
    

    public async Task<WindowsIdentityModel?> GetCache(string username)
    {
        try
        {
            return await _cacheProvider.GetObject<WindowsIdentityModel>(username);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }

    }
    
    public async Task<WindowsIdentityModel?> GetCache()
    {
        return (await _cacheProvider.GetAllObjects<WindowsIdentityModel>()).FirstOrDefault();
    }

    public void SetCache(WindowsIdentityModel userinfo)
    {
        _cacheProvider.Invalidate(userinfo.UserName);
        _cacheProvider.InsertObject(userinfo.UserName, userinfo);
    }
    
    public bool TryLogon(string userName, string password, string machineName, out string message)
    {
        var tokenHandle = nint.Zero;
        try
        {
            const int LOGON32_PROVIDER_DEFAULT = 0;
            const int LOGON32_LOGON_INTERACTIVE = 2;

            var returnValue = LogonUser(userName, 
                                        machineName, 
                                        password, 
                                        LOGON32_LOGON_INTERACTIVE,
                                        LOGON32_PROVIDER_DEFAULT, 
                                        out tokenHandle);

            if (returnValue == false)
            {
                var code = Marshal.GetLastWin32Error();
                if (code == 1327)
                {
                    message = "empty password!";
                    _logger.LogWarning("LogonUser:{message}", message);
                    return true;
                }
                else
                {
                    message = GetErrorMessage(code);
                    _logger.LogError("LogonUser:{message}",message);
                    return false;
                }
            }
            else
            {

                var windowsIdentity = new WindowsIdentity(tokenHandle);
                var principal = new WindowsPrincipal(windowsIdentity);
                message = principal.IsInRole(WindowsBuiltInRole.Administrator) ? "Access Granted. User is admin" : "Access Granted. User is not admin";
                return true;
            }
        }
        catch (Exception ex)
        {
            message = ex.Message;
            return false;
        }
        finally
        {
            if (tokenHandle != nint.Zero)
            {
                CloseHandle(tokenHandle);
            }
        }
    }

    public bool IsAdminGranted(string userName)
    {
        throw new NotImplementedException();
    }

    private static void GetCurrentUserAndMachineNameCore(out string userName, out string machineName)
    {
        var windowsIdentity = WindowsIdentity.GetCurrent();
        if (windowsIdentity.Name.Contains('\\'))
        {
            var names = windowsIdentity.Name.Split('\\');
            userName = names.Last();
            machineName = names.First();
        }
        else
        {
            userName = Environment.UserName;
            machineName = Environment.MachineName;
        }
    }
    private string GetErrorMessage(int errorCode)
    {
        var FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x100;
        var FORMAT_MESSAGE_IGNORE_INSERTS = 0x200;
        var FORMAT_MESSAGE_FROM_SYSTEM = 0x1000;

        var msgSize = 255;
        var lpMsgBuf = "";
        var dwFlags = FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS;

        var lpSource = nint.Zero;
        var lpArguments = nint.Zero;
        var returnVal = FormatMessage(dwFlags, ref lpSource, errorCode, 0, ref lpMsgBuf, msgSize, ref lpArguments);

        if (returnVal == 0)
        {
            throw new Exception("Failed to format message for error code " + errorCode + ". ");
        }

        return lpMsgBuf;

    }

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern bool LogonUser(string lpszUsername,
        string lpszDomain,
        string lpszPassword,
        int dwLogonType,
        int dwLogonProvider,
        out IntPtr phToken
    );

    [DllImport("kernel32.dll")]
    private static extern int FormatMessage(int dwFlags, ref IntPtr lpSource, int dwMessageId, int dwLanguageId,
        ref String lpBuffer, int nSize, ref IntPtr Arguments);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseHandle(IntPtr hObject);

    
}