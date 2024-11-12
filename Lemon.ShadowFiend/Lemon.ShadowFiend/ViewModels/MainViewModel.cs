using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Lemon.Avaloniaui.Extensions.Abstracts;
using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Core;

namespace Lemon.ShadowFiend.ViewModels;

public class MainViewModel : INavigationAware
{
    private readonly ITopLevelProvider _topLevelProvider;

    public MainViewModel(ITopLevelProvider topLevelProvider)
    {
        _topLevelProvider = topLevelProvider;
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        _topLevelProvider.SetMainWindowSize(900,600);
        _topLevelProvider.SetMainWindowCenterScreen();
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