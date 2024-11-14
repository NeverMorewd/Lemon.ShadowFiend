using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Lemon.Avaloniaui.Extensions.Abstracts;
using Lemon.ShadowFiend.ViewModels;

namespace Lemon.ShadowFiend.Views
{
    public partial class ShellWindow : Window
    {
        private readonly ITopLevelProvider _topLevelProvider;
        public ShellWindow(ITopLevelProvider topLevelProvider)
        {
            InitializeComponent();
            _topLevelProvider = topLevelProvider;
        }

        protected override async void OnClosing(WindowClosingEventArgs e)
        {
            e.Cancel = true;
            if (DataContext is not AppViewModel viewModel) return;
            var allow = await viewModel.RaiseExitWarning();
            if (allow)
            {
                _topLevelProvider.Shutdown();
            }
        }
    }
}