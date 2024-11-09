using System.Text.Json;

namespace OcxHome
{
    public struct OcxCommand
    {
        public OcxCommand(string command, object data) 
        {
            Command = command;
            Data = data;
        }
        public string Command 
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
            return $"{Command}:{JsonSerializer.Serialize(Data)}";
        }
    }
}
