using Avalonia.Controls;
using Avalonia.Interactivity;
using Lemon.ModuleNavigation.Abstracts;

namespace Lemon.ShadowFiend.Views
{
    public partial class ShellWindow : Window
    {
        private readonly INavigationService _navigationService;
        public ShellWindow(INavigationService navigationService)
        {
            InitializeComponent();
            _navigationService = navigationService;
        }

        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);
            _navigationService.NavigateToView("MainRegion", "LogonView");
            SizeToContent = SizeToContent.WidthAndHeight;
        }
    }
}