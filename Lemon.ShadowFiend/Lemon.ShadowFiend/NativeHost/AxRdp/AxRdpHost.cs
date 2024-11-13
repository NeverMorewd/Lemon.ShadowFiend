using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;

namespace Lemon.ShadowFiend.NativeHost.AxRdp
{
    public class AxRdpHost : NativeControlHost
    {
        public static readonly StyledProperty<IAxRdpProvder> ImplementationProperty =
            AvaloniaProperty.Register<AxRdpHost, IAxRdpProvder>(nameof(Implementation), defaultValue: new AxRdpProvder(((App)Application.Current!)!.ServiceProvider));

        public IAxRdpProvder Implementation
        {
            get => GetValue(ImplementationProperty);
            set => SetValue(ImplementationProperty, value);
        }
        static AxRdpHost()
        {

        }

        public bool IsSecond { get; set; }

        protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
        {
            return Implementation?.CreateControl(IsSecond, 
                       parent, 
                       () => base.CreateNativeControlCore(parent)) ?? base.CreateNativeControlCore(parent);
        }

        protected override void DestroyNativeControlCore(IPlatformHandle control)
        {
            base.DestroyNativeControlCore(control);
            Implementation?.Dispose();
        }
    }

}
