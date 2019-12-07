using System;
using System.Windows.Media;
using PostSharp.Patterns.Model;

namespace Jasmine.Core.Chat.Model
{

    [NotifyPropertyChanged]
    public class ChatMessageModel
    {
        public int Id { get; set; }
        public Guid MessageId { get; set; } = Guid.NewGuid();
        public string Author { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
        public bool IsOriginNative { get; set; }
        public bool HasAnchor { get; set; }
        public NotificationType NotificationType  { get; set; }
        public ImageSource Glyph { get; set; } = null;

    }
}
