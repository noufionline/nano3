using Jasmine.Core.Chat.Model;
using PostSharp.Patterns.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jasmine.Core.Chat.Views
{
    [NotifyPropertyChanged]
    public class SampleChatViewModel
    {
        public SampleChatViewModel()
        {
            ChatMessages.Add(new ChatMessageModel
            {
                Author = "Batman",
                Message = "What do you think about the Batmobile?",
                Time = DateTime.Now,
                IsOriginNative = true
            });
            ChatMessages.Add(new ChatMessageModel
            {
                Author = "Batman",
                Message = "Coolest superhero ride?",
                Time = DateTime.Now,
                IsOriginNative = true
            });
            ChatMessages.Add(new ChatMessageModel
            {
                Author = "Superman",
                Message = "Only if you don't have superpowers :P",
                Time = DateTime.Now
            });
            ChatMessages.Add(new ChatMessageModel
            {
                Author = "Batman",
                Message = "I'm rich. That's my superpower.",
                Time = DateTime.Now,
                IsOriginNative = true
            });
            ChatMessages.Add(new ChatMessageModel
            {
                Author = "Superman",
                Message = ":D Lorem Ipsum something blah blah blah blah blah blah blah blah. Lorem Ipsum something blah blah blah blah.",
                Time = DateTime.Now
            });
            ChatMessages.Add(new ChatMessageModel
            {
                Author = "Batman",
                Message = "I have no feelings",
                Time = DateTime.Now,
                IsOriginNative = true
            });
            ChatMessages.Add(new ChatMessageModel
            {
                Author = "Batman",
                Message = "How's Martha?",
                Time = DateTime.Now,
                IsOriginNative = true
            });
        }

        public ObservableCollection<ChatMessageModel> ChatMessages { get; set; } = new ObservableCollection<ChatMessageModel>();
    }
}
