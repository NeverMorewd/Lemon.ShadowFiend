using Akavache;
using Avalonia.Controls;
using Lemon.Avaloniaui.Extensions.Abstracts;
using Lemon.ShadowFiend.Models;
using Lemon.ShadowFiend.ViewModels;
using R3;

namespace Lemon.ShadowFiend.Views
{
    public partial class ShellWindow : Window
    {
        private readonly ITopLevelProvider _topLevelProvider;
        public ShellWindow(ITopLevelProvider topLevelProvider)
        {
            InitializeComponent();
            _topLevelProvider = topLevelProvider;
            AppContextModel.Current.Busy
                .ObserveOnUIThreadDispatcher()
                .Subscribe(b =>
                {
                    IsEnabled = !b;
                });
        }

        protected override async void OnClosing(WindowClosingEventArgs e)
        {
            e.Cancel = true;
            if (DataContext is not ShellViewModel viewModel) return;
            var allow = await viewModel.RaiseExitWarning();
            if (!allow) return;
            await BlobCache.Shutdown();
            _topLevelProvider.Shutdown();
        }
    }
}