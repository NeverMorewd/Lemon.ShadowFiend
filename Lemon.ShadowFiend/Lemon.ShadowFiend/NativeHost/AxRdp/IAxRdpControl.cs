using Avalonia.Platform;
using System;

namespace Lemon.ShadowFiend.NativeHost.AxRdp
{
    public interface IAxRdpControl
    {
        IPlatformHandle CreateControl(bool isSecond, IPlatformHandle parent, Func<IPlatformHandle> createDefault);
    }
}
