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
using Akavache;
using Lemon.Avaloniaui.Extensions;
using Lemon.Avaloniaui.Extensions.Abstracts;
using Lemon.ModuleNavigation.Avaloniaui.Dialogs;
using Lemon.ShadowFiend.Services;
using R3;
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
            Registrations.Start("Lemon.ShadowFiend");
            hostBuilder.Logging.ClearProviders();
            hostBuilder.Logging.AddConsole();
            hostBuilder.Logging.SetMinimumLevel(LogLevel.Debug);
            hostBuilder.Services.AddKeyedTransient<IAvaDialogWindow,DialogWindow>(DialogWindow.Key);
            hostBuilder.Services.AddAvaNavigationSupport();
            hostBuilder.Services.AddSingleton(BlobCache.UserAccount);
            hostBuilder.Services.AddSingleton<WindowsIdentityService>();
            hostBuilder.Services.AddSingleton<ITopLevelProvider, TopLevelProvider>();
            hostBuilder.Services.AddView<LogonView, LogonViewModel>(nameof(LogonView));
            hostBuilder.Services.AddView<MainView, MainViewModel>(nameof(MainView));
            hostBuilder.Services.AddView<ExitWarnView, ExitWarnViewModel>("ExitWarn");
            hostBuilder.Services.AddTransient<IAxRdpHome, AxMsRdpHomeForm>();
            hostBuilder.Services.AddAvaloniauiDesktopApplication<App>(BuildAvaloniaApp);
            hostBuilder.Services.AddMainWindow<ShellWindow, ShellViewModel>();
            var appHost = hostBuilder.Build();
            ObservableSystem.RegisterUnhandledExceptionHandler(Console.WriteLine);
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
