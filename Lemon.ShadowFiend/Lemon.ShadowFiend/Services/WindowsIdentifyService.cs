using System;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading.Tasks;
using Akavache;
using Lemon.ShadowFiend.Models;
using Microsoft.Extensions.Logging;
using Platform.Invoke.Win32.Kernel32;

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

            var returnValue = Kernel32Declaration.LogonUser(userName, 
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
                    message = ErrorMessageHelper.GetErrorMessage(code);
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
                Kernel32Declaration.CloseHandle(tokenHandle);
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
}