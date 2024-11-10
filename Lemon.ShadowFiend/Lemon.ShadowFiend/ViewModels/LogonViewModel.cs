using System;
using System.Reactive;
using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Core;
using Microsoft.Extensions.Logging;
using ReactiveUI;

namespace Lemon.ShadowFiend.ViewModels;

public class LogonViewModel : ViewModelBase, IDialogAware, INavigationAware
{
    private readonly ILogger _logger;
    private readonly INavigationService _navigationService;
    public LogonViewModel(ILogger<LogonViewModel> logger,INavigationService navigationService)
    {
        _logger = logger;
        _navigationService = navigationService;
        LogonCommand = ReactiveCommand.Create(() =>
        {
            _navigationService.NavigateToView("MainRegion","MainView");
        });
    }

    public ReactiveCommand<Unit, Unit> LogonCommand
    {
        get;
    }
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
        //throw new NotImplementedException();
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