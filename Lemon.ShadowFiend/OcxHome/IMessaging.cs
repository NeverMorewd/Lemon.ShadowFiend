namespace OcxHome
{
    public interface IMessaging
    {
        event Action<OcxMessage> Messaging;
    }
}
