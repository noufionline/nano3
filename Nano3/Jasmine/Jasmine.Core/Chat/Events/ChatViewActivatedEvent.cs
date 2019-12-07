using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jasmine.Core.Chat.Events
{
    public class ChatViewActivatedEvent : PubSubEvent<string>
    {
    }

    public class NewTextMessageEvent : PubSubEvent<TextMessage>
    {

    }

    public class TextMessage
    {
        public string Sender { get; set; }
        public Guid MessageId { get; set; }
        public string Message { get; set; }
        public MessageType MessageType { get; set; }
    }
}
