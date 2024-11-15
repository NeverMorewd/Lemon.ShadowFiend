using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Akavache;
using Avalonia.Controls.Notifications;
using Lemon.Avaloniaui.Extensions.Abstracts;
using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Core;
using Lemon.ShadowFiend.Models;
using Lemon.ShadowFiend.Services;
using Lemon.ShadowFiend.Utils;
using Microsoft.Extensions.Logging;
using Platform.Invoke.Win32.ChildSession;
using R3;

namespace Lemon.ShadowFiend.ViewModels;

public class LogonViewModel : ViewModelBase, INavigationAware
{
    private readonly ILogger _logger;
    private readonly INavigationService _navigationService;
    private readonly ITopLevelProvider _topLevelProvider;
    private readonly WindowsIdentityService _windowsIdentifyService;
    private readonly IBlobCache _cacheProvider;

    public LogonViewModel(ILogger<LogonViewModel> logger,
        INavigationService navigationService,
        ITopLevelProvider topLevelProvider,
        WindowsIdentityService windowsIdentifyService,
        IBlobCache cacheProvider)
    {
        _logger = logger;
        _navigationService = navigationService;
        _topLevelProvider = topLevelProvider;
        _windowsIdentifyService = windowsIdentifyService;
        _cacheProvider = cacheProvider;
        UserName = new BindableReactiveProperty<string>("").EnableValidation();
        Password = new BindableReactiveProperty<string>("").EnableValidation();
        ServerName = new BindableReactiveProperty<string>("").EnableValidation();
        ServerNameVisible = new BindableReactiveProperty<bool>();
        LogonCommand = new ReactiveCommand<ReadOnlyCollection<object>, (bool, string)>(LogonExecute, AwaitOperation.Sequential);
        LogonCommand.ChangeCanExecute(false);
        LogonCommand
            .SubscribeOnSynchronize(this)
            .SubscribeAwait(async (r, token) =>
            {
                if (r.Item1)
                {
                    _navigationService.RequestViewNavigation("MainRegion", "MainView", BuildParameters());
                }
                else
                {
                    // System.ArgumentException: An item with the same key has already been added. Key: Invalid username or password
                    _topLevelProvider.Ensure();
                    _topLevelProvider.NotificationManager!.Show("Invalid username or password",
                        NotificationType.Error,TimeSpan.FromSeconds(2));
                    await Task.Delay(TimeSpan.FromSeconds(2), token);
                }
            }, maxConcurrent: 1);
        
        Observable.CombineLatest(        
            UserName
                .Select(name => !string.IsNullOrEmpty(name))
                .Do(r =>
                {
                    if (!r)
                    {
                        UserName.OnErrorResume(new Exception("Please enter a valid username"));
                    }
                }),
             Password
                .Select(password => !string.IsNullOrEmpty(password))
                .Do(r =>
                {
                    if (!r)
                    {
                        Password.OnErrorResume(new Exception("Please enter a valid password"));
                    }
                }),
            ServerName
                .Select(serverName =>
                {
                    if (AppContextModel.Current.CurrentRdpType.Value == RdpType.RemoteSession)
                    {
                        return !string.IsNullOrEmpty(serverName);
                    }
                    return true;
                })
                .Do(r =>
                {
                    if (!r)
                    {
                        ServerName.OnErrorResume(new Exception("Please enter a valid servername"));
                    }
                }),
            (isValidUserName, isValidPassword, isValidServerName) => isValidUserName && isValidPassword && isValidServerName)
            .Subscribe(canExecute => { LogonCommand.ChangeCanExecute(canExecute); });

        AppContextModel.Current.CurrentRdpType.SubscribeAwait(OnNext);
    }

   

    public BindableReactiveProperty<string> UserName { get; }
    public BindableReactiveProperty<string> Password { get; }
    public BindableReactiveProperty<string> ServerName { get; }
    public BindableReactiveProperty<bool> ServerNameVisible { get; }
    public ReactiveCommand<ReadOnlyCollection<object>, (bool, string)> LogonCommand { get; }
    private async ValueTask OnNext(RdpType type, CancellationToken token)
    {
        if (type == RdpType.ChildSession)
        {
            ServerNameVisible.Value = false;
            UserName.Value = (await _windowsIdentifyService.GetCurrentUserName())!;
            var userInfo = await _windowsIdentifyService.GetCache(UserName.Value);
            if (userInfo.HasValue)
            {
                Password.Value = userInfo.Value.Password;
            }

            if (ChildSessionHelper.IsSupportChildSession())
            {
                if (!ChildSessionHelper.IsEnableChildSession())
                {
                    var enable = ChildSessionHelper.EnableChildSession();
                    if (!enable)
                    {
                        _topLevelProvider.NotificationManager!.Show(
                            "Fail to enable child session!Please restart with Administrator!", NotificationType.Error,
                            TimeSpan.FromSeconds(5));
                    }
                }
            }
        }
        else
        {
            ServerNameVisible.Value = true;
            ServerName.OnNext("");
        }
    }
    private async ValueTask<(bool, string)> LogonExecute(ReadOnlyCollection<object> input,
        CancellationToken cancellationToken)
    {
        if (AppContextModel.Current.CurrentRdpType.Value == RdpType.RemoteSession)
        {
            return (true, "");
        }
        AppContextModel.Current.Busy.Value = true;
        try
        {
            var machineName = await _windowsIdentifyService.GetCurrentMachineName();
            return await Task.Run(() =>
            {
                var userInfo = input.Cast<string>().ToArray();
                var result = _windowsIdentifyService.TryLogon(userInfo[0], userInfo[1], machineName!, out var message);
                return (result, message);
            }, cancellationToken);
        }
        finally
        {
            AppContextModel.Current.Busy.Value = false;
        }
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        _topLevelProvider.SetMainWindowSize(400, 500);
        _topLevelProvider.SetMainWindowCenterScreen();
        _topLevelProvider.Ensure();
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public async void OnNavigatedFrom(NavigationContext navigationContext)
    {
        _windowsIdentifyService.SetCache(new WindowsIdentityModel(UserName.Value,
            Password.Value,
            (await _windowsIdentifyService.GetCurrentMachineName())!));
    }

    private NavigationParameters BuildParameters()
    {
        var parameters = new NavigationParameters
        {
            { "UserName", UserName.Value.ToSecureString() },
            { "Password", Password.Value.ToSecureString() },
            { "ServerName", ServerName.Value }
        };
        return parameters;
    }
}