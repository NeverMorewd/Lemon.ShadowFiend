using System;
using System.Reactive;
using System.Reactive.Linq;
using Akavache;
using Lemon.Avaloniaui.Extensions.Abstracts;
using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Core;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace Lemon.ShadowFiend.ViewModels;

public class LogonViewModel : ViewModelBase, IDialogAware, INavigationAware
{
    private readonly ILogger _logger;
    private readonly INavigationService _navigationService;
    private readonly ITopLevelProvider _topLevelProvider;

    public LogonViewModel(ILogger<LogonViewModel> logger, INavigationService navigationService,
        ITopLevelProvider topLevelProvider)
    {
        _logger = logger;
        _navigationService = navigationService;
        _topLevelProvider = topLevelProvider;
        LogonCommand = ReactiveCommand.Create(() =>
        {
            BlobCache.UserAccount.InsertObject("toaster", 1);
            _navigationService.RequestViewNavigation("MainRegion", "MainView");
            var re = BlobCache.UserAccount.GetObject<int>("toaster").Wait();
        });
    }

    public ReactiveCommand<Unit, Unit> LogonCommand { get; }
    public string Title => "Logon";
    public event Action<IDialogResult>? RequestClose;

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
        _topLevelProvider.SetMainWindowSize(300,400);
        _topLevelProvider.SetMainWindowCenterScreen();
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        //throw new NotImplementedException();
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
        //throw new NotImplementedException();
    }
}