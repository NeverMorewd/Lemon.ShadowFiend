using Avalonia.Controls;
using Avalonia.Controls.Notifications;

namespace Lemon.Avaloniaui.Extensions.Abstracts;

public interface ITopLevelProvider
{
    WindowNotificationManager? NotificationManager { get; }
    TopLevel Ensure(TimeSpan timespan = default);
    TopLevel? Get();
    Window? MainWindow { get; }
    void SetMainWindowCenterScreen(int screenNo = 1);
    void SetMainWindowSize(int width, int height);
}