using Avalonia;
using Avalonia.Markup.Xaml;
using System;
using Lemon.ModuleNavigation.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace Lemon.ShadowFiend
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        public IServiceProvider ServiceProvider { get; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            base.OnFrameworkInitializationCompleted();
            var dialogService = ServiceProvider.GetRequiredService<IDialogService>();
            //dialogService.Show("LogonView");
        }
    }
}