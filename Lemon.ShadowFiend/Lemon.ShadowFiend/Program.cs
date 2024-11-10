using Avalonia;
using Avalonia.ReactiveUI;
using Lemon.Hosting.AvaloniauiDesktop;
using Lemon.ModuleNavigation.Avaloniaui;
using Lemon.ShadowFiend.ViewModels;
using Lemon.ShadowFiend.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OcxHome.AxMsRdpHome;
using System;
using System.Runtime.Versioning;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Lemon.ShadowFiend
{
    internal static class Program
    {
        [STAThread]
        [SupportedOSPlatform("windows")]
        public static void Main(string[] args)
        {
            var hostBuilder = Host.CreateApplicationBuilder();

            hostBuilder.Logging.ClearProviders();
            hostBuilder.Logging.AddConsole();
            hostBuilder.Logging.SetMinimumLevel(LogLevel.Debug);

            hostBuilder.Services.AddAvaNavigationSupport();
            hostBuilder.Services.AddView<LogonView, LogonViewModel>(nameof(LogonView));
            hostBuilder.Services.AddView<MainView, MainViewModel>(nameof(MainView));
            hostBuilder.Services.AddTransient<IAxRdpHome, AxMsRdpHomeForm>();
            hostBuilder.Services.AddAvaloniauiDesktopApplication<App>(BuildAvaloniaApp);
            hostBuilder.Services.AddMainWindow<ShellWindow, AppViewModel>();
            var appHost = hostBuilder.Build();
            appHost.RunAvaloniauiApplication<ShellWindow>(args);
        }

        private static AppBuilder BuildAvaloniaApp(AppBuilder appBuilder)
        {
            return appBuilder
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI();
        }
    }
}
