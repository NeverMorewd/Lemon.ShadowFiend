using System.Text.Json;

namespace OcxHome
{
    public struct OcxMessage
    {
        public OcxMessage(string messageType, object data)
        {
            MessageType = messageType;
            Data = data;
        }
        public OcxMessage(Exception exception)
        {
            MessageType = exception.GetType().Name;
            Data = exception;
        }
        public string MessageType
        {
            get;
            private set;
        }
        public object Data
        {
            get;
            private set;
        }
        public override string ToString()
        {
            return $"{MessageType}:{JsonSerializer.Serialize(Data)}";
        }
    }
}
