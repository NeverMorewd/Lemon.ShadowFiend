using System;
using System.Security;
using Avalonia.Controls.ApplicationLifetimes;
using Lemon.Avaloniaui.Extensions.Abstracts;
using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Core;
using Lemon.ShadowFiend.Models;
using Lemon.ShadowFiend.NativeHost.AxRdp;
using Lemon.ShadowFiend.Utils;
using R3;

namespace Lemon.ShadowFiend.ViewModels;

public class MainViewModel : INavigationAware
{
    private readonly ITopLevelProvider _topLevelProvider;
    private readonly BehaviorSubject<bool> _isAxRdpInitializedSubject;
    private readonly IDisposable _disposable;

    public MainViewModel(ITopLevelProvider topLevelProvider)
    {
        _topLevelProvider = topLevelProvider;
        _isAxRdpInitializedSubject = new BehaviorSubject<bool>(false);
        var disposableBuilder = Disposable.CreateBuilder();
        Implementation = new BindableReactiveProperty<IAxRdpProvder?>();
        Implementation
            .Where(impl=> impl != null)
            .Subscribe(impl =>
            {
                impl!.OnInitialized += _ =>
                {
                    _isAxRdpInitializedSubject.OnNext(true);
                };
            }).AddTo(ref disposableBuilder);
        _topLevelProvider.DesktopLifetime.ShutdownRequested += ShutdownRequestedHandler;
        _disposable = disposableBuilder.Build();
    }
    
    public BindableReactiveProperty<IAxRdpProvder?> Implementation
    {
        get;
    }

    private void ShutdownRequestedHandler(object? obj, ShutdownRequestedEventArgs? eventArgs)
    {
        Clean();
        _topLevelProvider.DesktopLifetime.ShutdownRequested -= ShutdownRequestedHandler;
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        var parameters = navigationContext.Parameters!;
        var userName = parameters.GetValue<SecureString>("UserName")!.SecureStringToString();
        var pwd = parameters.GetValue<SecureString>("Password")!.SecureStringToString();
        _topLevelProvider.SetMainWindowSize(950,600);
        _topLevelProvider.SetMainWindowCenterScreen();
        _isAxRdpInitializedSubject
            .Where(isInitialized => isInitialized)
            .Take(1)
            .Subscribe(_ =>
            {
                if (AppContextModel.Current.CurrentRdpType.Value == RdpType.ChildSession)
                {
                    Implementation.Value?.ConntectToChildSession(userName!, pwd!);
                }
                else
                {
                    var serverName = parameters.GetValue<string>("ServerName")!;
                    Implementation.Value?.Connect(serverName!, userName!, pwd!);
                }
            });

    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
        Clean();
    }

    private void Clean()
    {
        _isAxRdpInitializedSubject.Dispose();
        _disposable.Dispose();
        Implementation.Value?.Disconnect();
        if (AppContextModel.Current.CurrentRdpType.Value == RdpType.ChildSession)
        {
            Implementation.Value?.LogoutChildSession();
        }
        Implementation.Value?.Dispose();
    }
}