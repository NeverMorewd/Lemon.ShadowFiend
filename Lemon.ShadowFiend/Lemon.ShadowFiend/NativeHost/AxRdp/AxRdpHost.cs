using Avalonia.Controls;
using Avalonia.Platform;

namespace Lemon.ShadowFiend.NativeHost.AxRdp
{
    public class AxRdpHost : NativeControlHost
    {
        public static IAxRdpControl? Implementation { get; set; }

        static AxRdpHost()
        {
            Implementation = new AxRdpControlImpl(((App)Avalonia.Application.Current!)!.ServiceProvider);
        }

        public bool IsSecond { get; set; }

        protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
        {
            return Implementation?.CreateControl(IsSecond, parent, () => base.CreateNativeControlCore(parent))
                ?? base.CreateNativeControlCore(parent);
        }

        protected override void DestroyNativeControlCore(IPlatformHandle control)
        {
            base.DestroyNativeControlCore(control);
        }
    }

}
