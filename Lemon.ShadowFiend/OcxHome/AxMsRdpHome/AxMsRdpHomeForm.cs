using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MSTSCLib;
using System.ComponentModel;
using Platform.Invoke.Win32.ChildSession;

namespace OcxHome.AxMsRdpHome
{
    public partial class AxMsRdpHomeForm : Form, IAxRdpHome, IMessaging
    {
        private IDisposable? _disposedForCommand;
        private readonly ILogger _logger;
        public AxMsRdpHomeForm(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _logger = serviceProvider.GetRequiredService<ILogger<AxMsRdpHomeForm>>();
        }
        public event Action<OcxMessage>? Messaging;
        public IObservable<OcxMessage> MessageStream => axMsRdpClient8.CreateEventsStream(this);

        public nint OcxHandle => axMsRdpClient8.Handle;

        public nint Hwnd => Handle;

        public void SetCommandStream(IObservable<OcxCommand> commandSteam)
        {
            _disposedForCommand = commandSteam.Subscribe(cmd => 
            {
                _logger.LogDebug(cmd.ToString());
            });
        }
        public void Unload()
        {
            axMsRdpClient8.Disconnect();
            axMsRdpClient8.Dispose();
            Close();
            ///GC.Collect();
        }
        void IAxRdpHome.Load()
        {
            Show();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AxMsRdpInitialize();
            Messaging?.Invoke(new OcxMessage(nameof(OnLoad), e));
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            _disposedForCommand?.Dispose();
        }
        private void AxMsRdpInitialize()
        {
            
        }
        private void ConnectCore(string remoteServer, string userName, string pwd, bool childSession = false)
        {
            try
            {
                ArgumentException.ThrowIfNullOrEmpty(remoteServer, userName);
                if (string.IsNullOrEmpty(pwd))
                {
                    throw new ArgumentException("Empty password is not allowed!");
                }
                var msRdpClient7 = axMsRdpClient8.GetOcx() as IMsRdpClient7;
                var advancedSettings = msRdpClient7!.AdvancedSettings7;
                axMsRdpClient8.Server = remoteServer;
                axMsRdpClient8.UserName = userName;
                advancedSettings.ClearTextPassword = pwd;

                if (childSession)
                {
                    IMsRdpExtendedSettings? settings = axMsRdpClient8.GetOcx() as IMsRdpExtendedSettings;
                    object otrue = true;
                    settings!.set_Property("ConnectToChildSession", ref otrue);
                }
                
                advancedSettings.PublicMode = false;
                advancedSettings.EnableCredSspSupport = true;
                advancedSettings.SmartSizing = true;
                advancedSettings.DisplayConnectionBar = false;
                advancedSettings.RedirectSmartCards = true;
                //_advancedSettings.PluginDlls = GetInstalledPluginDlls();

                axMsRdpClient8.DesktopWidth = Screen.PrimaryScreen!.Bounds.Width;
                axMsRdpClient8.DesktopHeight = Screen.PrimaryScreen!.Bounds.Height;
                axMsRdpClient8.Connect();
            }
            catch (Exception ex)
            {
                Messaging?.Invoke(new OcxMessage(ex));
            }
        }

        void IAxRdpHome.Connect(string server, string name, string pwd, bool childSession)
        {
            ConnectCore(server, name, pwd, childSession);
        }

        public void Disconnect()
        {
            axMsRdpClient8?.Disconnect();
        }

        public Task Logout()
        {
            return 
            Task.Factory.StartNew(() =>
            {
                ChildSessionHelper.LogOutChildSession();
            },TaskCreationOptions.LongRunning);
        }
    }
}
