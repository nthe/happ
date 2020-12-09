using System;

namespace server.Models
{
     public interface IRecipient
    {
        string Resource { get; }

        void HandleDelivery(Envelope message);
    }
    
    public class Message
    {
        public string Resource { get; set; }

        public string[] Args { get; set; }
    }
    
    public class Envelope
    {
        public Message Data { get; set; }

        public Action<string> SendResponse { get; set; }
    }
}