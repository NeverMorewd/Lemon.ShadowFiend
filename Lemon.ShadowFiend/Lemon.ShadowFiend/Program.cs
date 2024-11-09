using Avalonia;
using Avalonia.ReactiveUI;
using Lemon.Hosting.AvaloniauiDesktop;
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

            hostBuilder.Services.AddTransient<IAxRdpHome, AxMsRdpHomeForm>();
            hostBuilder.Services.AddAvaloniauiDesktopApplication<App>(BuildAvaloniaApp);
            hostBuilder.Services.AddMainWindow<MainWindow, MainWindowViewModel>();
            var appHost = hostBuilder.Build();
            appHost.RunAvaloniauiApplication<MainWindow>(args);
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
