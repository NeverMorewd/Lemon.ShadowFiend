using Avalonia.Platform;
using System;

namespace Lemon.ShadowFiend.NativeHost.AxRdp
{
    public interface IAxRdpProvder : IDisposable
    {
        event Action<nint>? OnInitialized;
        void Connect(string server, string userName, string password);
        void ConntectToChildSession(string userName, string password);
        void Disconnect();
        IPlatformHandle CreateControl(bool isSecond, IPlatformHandle parent, Func<IPlatformHandle> createDefault);
    }
}
