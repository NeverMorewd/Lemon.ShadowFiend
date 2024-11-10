using System;
using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ShadowFiend.ViewModels
{
    public class AppViewModel : ViewModelBase, IServiceAware
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        public AppViewModel(IServiceProvider serviceProvider, IDialogService dialogService,
            INavigationService navigationService)
        {
            _serviceProvider = serviceProvider;
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        public IServiceProvider ServiceProvider => _serviceProvider;
    }
}
