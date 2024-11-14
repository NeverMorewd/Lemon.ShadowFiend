using System;
using System.Threading;
using System.Threading.Tasks;
using Lemon.ModuleNavigation.Abstracts;
using Lemon.ModuleNavigation.Dialogs;
using Lemon.ShadowFiend.Models;
using Lemon.ShadowFiend.Views;
using R3;

namespace Lemon.ShadowFiend.ViewModels
{
    public class AppViewModel : ViewModelBase, IServiceAware
    {
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        public AppViewModel(IServiceProvider serviceProvider, IDialogService dialogService,
            INavigationService navigationService)
        {
            ServiceProvider = serviceProvider;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _navigationService.RequestViewNavigation("MainRegion", "LogonView");
        }

        public IServiceProvider ServiceProvider { get; }

        public async ValueTask<bool> RaiseExitWarning()
        {
            var allow = false;
            await _dialogService.ShowDialog("ExitWarn",DialogWindow.Key, callback: (result) =>
            {
                allow = result.Result == ButtonResult.Yes;
            });
            return allow;
        }
    }
}
