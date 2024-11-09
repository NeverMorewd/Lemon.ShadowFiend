using AxMSTSCLib;
using System.Reactive.Linq;

namespace OcxHome.AxMsRdpHome
{
    public static class AxMsRdpClient8Extensions
    {
        public static IObservable<OcxMessage> CreateEventsStream(
            this AxMsRdpClient8NotSafeForScripting rdpClient, 
            IMessaging messagingForm)
        {
            var messaging = Observable.FromEvent<Action<OcxMessage>, OcxMessage>(
                h => messagingForm.Messaging += h, 
                h => messagingForm.Messaging -= h);

            var loginComplete = Observable.FromEventPattern(
                h => rdpClient.OnLoginComplete += h,
                h => rdpClient.OnLoginComplete -= h)
                .Select(e => new OcxMessage("LoginComplete", e.EventArgs));

            var authWarning = Observable.FromEventPattern(
                h => rdpClient.OnAuthenticationWarningDisplayed += h,
                h => rdpClient.OnAuthenticationWarningDisplayed -= h)
                .Select(e => new OcxMessage("AuthWarning", e.EventArgs));

            var warning = Observable.FromEventPattern<IMsTscAxEvents_OnWarningEventHandler, IMsTscAxEvents_OnWarningEvent>(
                h => rdpClient.OnWarning += h,
                h => rdpClient.OnWarning -= h)
                .Select(e => new OcxMessage("Warning", e.EventArgs));

            var connecting = Observable.FromEventPattern(
                h => rdpClient.OnConnecting += h,
                h => rdpClient.OnConnecting -= h)
                .Select(e => new OcxMessage("Connecting", e.EventArgs));

            var connected = Observable.FromEventPattern(
                h => rdpClient.OnConnected += h,
                h => rdpClient.OnConnected -= h)
                .Select(e => new OcxMessage("Connected", e.EventArgs));

            var disconnected = Observable.FromEventPattern<IMsTscAxEvents_OnDisconnectedEventHandler, IMsTscAxEvents_OnDisconnectedEvent>(
                  h => rdpClient.OnDisconnected += h,
                  h => rdpClient.OnDisconnected -= h)
                  .Select(e => new OcxMessage("Warning", e.EventArgs));

            var logonError = Observable.FromEventPattern<IMsTscAxEvents_OnLogonErrorEventHandler, IMsTscAxEvents_OnLogonErrorEvent>(
                h => rdpClient.OnLogonError += h,
                h => rdpClient.OnLogonError -= h)
                .Select(e => new OcxMessage("LogonError", e.EventArgs));

            var fatalError = Observable.FromEventPattern<IMsTscAxEvents_OnFatalErrorEventHandler, IMsTscAxEvents_OnFatalErrorEvent>(
                h => rdpClient.OnFatalError += h,
                h => rdpClient.OnFatalError -= h)
                .Select(e => new OcxMessage("FatalError", e.EventArgs));

            return Observable.Merge(messaging,
                                    loginComplete,
                                    authWarning,
                                    warning,
                                    connecting,
                                    connected,
                                    disconnected,
                                    logonError,
                                    fatalError);
        }
    }

}
