namespace OcxHome.AxMsRdpHome
{
    public interface IAxRdpHome
    {
        IObservable<OcxMessage> MessageStream { get; }
        nint OcxHandle { get; }
        nint Hwnd { get; }
        void SetCommandStream(IObservable<OcxCommand> commandSteam);
        void Load();
        void Unload();
        void Connect(string server, string name, string pwd, bool childSession);
    }
}
