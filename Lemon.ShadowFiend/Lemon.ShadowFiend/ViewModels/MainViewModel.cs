using System;
using System.Security;
using Lemon.Avaloniaui.Extensions.Abstracts;
using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Core;
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
        _disposable = disposableBuilder.Build();
    }

    public BindableReactiveProperty<IAxRdpProvder?> Implementation
    {
        get;
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
                Implementation.Value?.ConntectToChildSession(userName!, pwd!);
            });

    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
        _isAxRdpInitializedSubject.Dispose();
        _disposable.Dispose();
        Implementation.Value?.Disconnect();
        Implementation.Value?.Dispose();
    }
}