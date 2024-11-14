using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using Avalonia.Platform;
using Lemon.Avaloniaui.Extensions.Abstracts;

namespace Lemon.Avaloniaui.Extensions;

public class TopLevelProvider : ITopLevelProvider
{
    private TopLevel? _topLevel;
    private WindowNotificationManager? _notificationManager;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private IClassicDesktopStyleApplicationLifetime? _desktopLifetime;

    public TopLevelProvider()
    {
        
    }
    public WindowNotificationManager? NotificationManager
    {
        get
        {
            if (_notificationManager != null) return _notificationManager;
            if (_topLevel == null)
            {
                Ensure();
            }
            _notificationManager = new WindowNotificationManager(_topLevel)
            {
                MaxItems = 30,
                Position = NotificationPosition.BottomCenter,
                /*Width = 100,
                Height = 50,
                Padding = new Thickness(0)*/
            };
            return _notificationManager;
        }
    }

    public IClassicDesktopStyleApplicationLifetime DesktopLifetime
    {
        get
        {
            Ensure();
            return _desktopLifetime!;
        }
    }

    public TopLevel? Get()
    {
        return GetTopLevelCore();
    }

    public Window? MainWindow
    {
        get
        {
            if (Application.Current is null) return null;
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                return desktop.MainWindow;
            }
            throw new InvalidOperationException($"Can not get MainWindow from not-desktoplifetime!");
        }
    }

    public void Shutdown()
    {
        Ensure();
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
        else
        {
            throw new InvalidOperationException($"Can not excute shutdown with not-desktoplifetime!");
        }
        
    }

    public TopLevel Ensure(TimeSpan timespan = default)
    {
        var startTime = DateTime.UtcNow;
        while (true)
        {
            try
            {
                _semaphore.Wait();
                var topLevel = GetTopLevelCore();
                if (topLevel != null)
                {
                    return topLevel;
                }

                if (timespan == default)
                {
                    throw new InvalidOperationException($"Fail to get TopLevel!");
                }
                if (DateTime.UtcNow - startTime > timespan)
                {
                    throw new TimeoutException($"Fail to get TopLevel within {timespan.TotalSeconds} seconds");
                }

                Thread.Sleep(10);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }

    public void SetMainWindowLocation(int x, int y, int screenNo = 1)
    {
        if (MainWindow != null) MainWindow.Position = new PixelPoint(x, y);
    }

    public void SetMainWindowCenterScreen(int screenNo = 1)
    {
        Screen? screen = null;
        var screens = Get()?.Screens;
        if (screens == null) return;
        screen = screenNo == 1 ? screens.Primary : screens.All[screenNo];
        if (screen == null) return;
        var left = (int)(screen.WorkingArea.Width - MainWindow!.Width) / 2;
        var top = (int)(screen.WorkingArea.Height - MainWindow!.Height) / 2;
        MainWindow.Position = new PixelPoint(left, top);
    }

    public void SetMainWindowSize(int width, int height)
    {
        if (MainWindow == null) return;
        MainWindow.SizeToContent = SizeToContent.Manual;
        MainWindow.Height = height;
        MainWindow.Width = width;
    }

    public void Dispose()
    {
        _semaphore.Dispose();
    }
    
    
    private TopLevel? GetTopLevelCore()
    {
        if (_topLevel != null)
        {
            return _topLevel;
        }

        if (Application.Current is null) return null;
        switch (Application.Current.ApplicationLifetime)
        {
            case IClassicDesktopStyleApplicationLifetime { MainWindow: not null } desktop:
                _topLevel = TopLevel.GetTopLevel(desktop.MainWindow);
                _desktopLifetime = desktop;
                return _topLevel;
            case ISingleViewApplicationLifetime { MainView: not null } single:
                _topLevel = TopLevel.GetTopLevel(single.MainView);
                return _topLevel;
            default:
                return null;
        }
    }
}