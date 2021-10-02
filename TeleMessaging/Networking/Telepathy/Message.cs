using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleMessaging.Networking.Telepathy
{
    public class Message
    {
        public int connectionId;
        public byte[] data;
        public EventType eventType;

        public Message(int connectionId, EventType eventType, byte[] data)
        {
            this.connectionId = connectionId;
            this.eventType = eventType;
            this.data = data;
        }
    }
}
