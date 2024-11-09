using Avalonia.Platform;
using Microsoft.Extensions.DependencyInjection;
using OcxHome.AxMsRdpHome;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace Lemon.ShadowFiend.NativeHost.AxRdp
{
    public class AxRdpControlImpl : IAxRdpControl
    {
        private IAxRdpHome? _axRdpHome;
        private readonly IServiceProvider _serviceProvider;
        public AxRdpControlImpl(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }
        public IPlatformHandle CreateControl(bool isSecond, IPlatformHandle parent, Func<IPlatformHandle> createDefault)
        {
            _axRdpHome = _serviceProvider.GetRequiredService<IAxRdpHome>();
            _axRdpHome.MessageStream.ObserveOn(RxApp.MainThreadScheduler).Subscribe(m => 
            {
                if (m.MessageType == "OnLoad")
                {
                    //_axRdpHome.Connect("localhost","@hotmail.com","",true);
                }
            });
            _axRdpHome.Load();
            return new Win32WindowControlHandle(_axRdpHome.Hwnd, "HWND");
        }
    }
}
