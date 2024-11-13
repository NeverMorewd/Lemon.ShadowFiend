using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Akavache;
using Lemon.Avaloniaui.Extensions.Abstracts;
using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Core;
using Lemon.ShadowFiend.Models;
using Lemon.ShadowFiend.Services;
using Lemon.ShadowFiend.Utils;
using Microsoft.Extensions.Logging;
using R3;

namespace Lemon.ShadowFiend.ViewModels;

public class LogonViewModel : ViewModelBase, IDialogAware, INavigationAware
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
        UserName = new BindableReactiveProperty<string>().EnableValidation();
        Password = new BindableReactiveProperty<string>().EnableValidation();
        LogonCommand = new ReactiveCommand<ReadOnlyCollection<object>, (bool, string)>(LogonExecute, AwaitOperation.Sequential);
        LogonCommand.ChangeCanExecute(false);
        LogonCommand.Subscribe(r =>
        {
            if (r.Item1)
            {
                _navigationService.RequestViewNavigation("MainRegion", "MainView", BuildParameters());
            }
        });
        UserName.Select(name => !string.IsNullOrEmpty(name))
            .Do(r =>
            {
                if (!r)
                {
                    UserName.OnErrorResume(new Exception("Please enter a valid username"));
                }
            })
            .CombineLatest(Password
                    .Select(password => !string.IsNullOrEmpty(password))
                    .Do(r =>
                    {
                        if (!r)
                        {
                            Password.OnErrorResume(new Exception("Please enter a valid password"));
                        }
                    }),
                (isValidUserName, isValidPassword) => isValidUserName && isValidPassword)
            .Subscribe(canExecute => { LogonCommand.ChangeCanExecute(canExecute); });

        AppContextModel.Current.CurrentRdpType.SubscribeAwait(OnNext);
    }

    private async ValueTask OnNext(RdpType type, CancellationToken token)
    {
        if (type == RdpType.ChildSession)
        {
            UserName.Value = (await _windowsIdentifyService.GetCurrentUserName())!;
            var userInfo = await _windowsIdentifyService.GetCache(UserName.Value);
            if (userInfo.HasValue)
            {
                Password.Value = userInfo.Value.Password;
            }
        }
    }

    public BindableReactiveProperty<string> UserName { get; }
    public BindableReactiveProperty<string> Password { get; }
    public ReactiveCommand<ReadOnlyCollection<object>, (bool, string)> LogonCommand { get; }
    public string Title => "Logon";
    public event Action<IDialogResult>? RequestClose;

    private async ValueTask<(bool, string)> LogonExecute(ReadOnlyCollection<object> input,
        CancellationToken cancellationToken)
    {
        var machineName = await _windowsIdentifyService.GetCurrentMachineName();
        var userInfo = input.Cast<string>().ToArray();
        var result = _windowsIdentifyService.TryLogon(userInfo[0], userInfo[1], machineName!, out var message);
        return (result, message);
    }

    public void OnDialogClosed()
    {
        //throw new NotImplementedException();
    }

    public void OnDialogOpened(IDialogParameters? parameters)
    {
        _logger.LogDebug($"Logon");
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        _topLevelProvider.SetMainWindowSize(300, 400);
        _topLevelProvider.SetMainWindowCenterScreen();
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        //throw new NotImplementedException();
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
            { "Password", Password.Value.ToSecureString() }
        };
        return parameters;
    }
}