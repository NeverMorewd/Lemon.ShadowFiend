using System;
using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Core;
using Lemon.ModuleNavigation.Dialogs;
using R3;

namespace Lemon.ShadowFiend.ViewModels;

public class ExitWarnViewModel : IDialogAware, INavigationAware
{
    public ExitWarnViewModel()
    {
        Title = "Warning";
        ConfirmCommand = new ReactiveCommand(_ =>
        {
            var result = new DialogResult(ButtonResult.Yes);
            RequestClose?.Invoke(result);
        });
        CancelCommand = new ReactiveCommand(_ =>
        {
            var result = new DialogResult(ButtonResult.Cancel);
            RequestClose?.Invoke(result);
        });
    }

    public ReactiveCommand ConfirmCommand { get; }
    public ReactiveCommand CancelCommand { get; }


    public void OnDialogClosed()
    {
        //
    }

    public void OnDialogOpened(IDialogParameters? parameters)
    {
        //
    }

    public string Title { get; }
    public event Action<IDialogResult>? RequestClose;

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        //throw new NotImplementedException();
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return false;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
        //throw new NotImplementedException();
    }
}