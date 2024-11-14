using Avalonia.Platform;
using Microsoft.Extensions.DependencyInjection;
using OcxHome.AxMsRdpHome;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using OcxHome;

namespace Lemon.ShadowFiend.NativeHost.AxRdp
{
    public class AxRdpProvder : IAxRdpProvder
    {
        private IAxRdpHome? _axRdpHome;
        private readonly IServiceProvider _serviceProvider;
        private bool _isInitialized = false;
        private IDisposable? _disposable;
        public AxRdpProvder(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        public event Action<nint>? OnInitialized;

        public async Task Logout()
        {
            if (_isInitialized)
            {
                await _axRdpHome!.Logout();
            }
        }

        IPlatformHandle IAxRdpProvder.CreateControl(bool isSecond, IPlatformHandle parent, Func<IPlatformHandle> createDefault)
        {
            try
            {
                _axRdpHome = _serviceProvider.GetRequiredService<IAxRdpHome>();
                _disposable = _axRdpHome.MessageStream
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(m => 
                {
                    if (m.MessageType == "OnLoad")
                    {
                        //_axRdpHome.Connect("localhost","@hotmail.com","",true);
                        _isInitialized = true;
                        OnInitialized?.Invoke(_axRdpHome.Hwnd);
                    }
                });
                _axRdpHome.Load();
                return new Win32WindowControlHandle(_axRdpHome.Hwnd, "HWND");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        public void Connect(string server, string userName, string password)
        {
            if (_isInitialized)
            {
                _axRdpHome!.Connect(server, userName, password, false);
            }
        }
        
        public void ConntectToChildSession(string userName, string password)
        {
            if (_isInitialized)
            {
                _axRdpHome!.Connect("localhost", userName, password, true);
            }
        }

        public void Disconnect()
        {
            if (_isInitialized)
            {
                _axRdpHome!.Disconnect();
            }
        }

    }
}
