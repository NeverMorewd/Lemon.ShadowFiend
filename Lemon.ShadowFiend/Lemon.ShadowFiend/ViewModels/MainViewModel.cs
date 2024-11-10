using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Core;

namespace Lemon.ShadowFiend.ViewModels;

public class MainViewModel:INavigationAware
{
    public MainViewModel()
    {
        
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime
            desktopStyleApplicationLifetime)
        {
            desktopStyleApplicationLifetime.MainWindow.SizeToContent = SizeToContent.Manual;
            desktopStyleApplicationLifetime.MainWindow.Width = 1000;
            desktopStyleApplicationLifetime.MainWindow.Height = 600;
        }
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        //throw new System.NotImplementedException();
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {
        //throw new System.NotImplementedException();
    }
}