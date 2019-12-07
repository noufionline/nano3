using System;
using System.Collections.ObjectModel;
using System.Linq;
using PostSharp.Patterns.Model;

namespace Jasmine.Core.Chat.Model
{
    [NotifyPropertyChanged]
    public class ParticipantModel
    {
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool HasUnreadMessage { get; set; }
    }

    public class UserModel
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public byte[] Photo { get; set; }
        public bool IsLoggedIn { get; set; }
    }
}
