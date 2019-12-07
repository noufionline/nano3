using Jasmine.Core.Chat.Model;
using Jasmine.Core.Contracts;
using Jasmine.Core.Mvvm;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Patterns.Model;
using DevExpress.Mvvm;
using Jasmine.Core.Chat.Extensions;
using System.IO;
using System.Security.Claims;
using Jasmine.Core.Chat.Events;
using System.Reactive.Linq;
using System.Timers;
using DevExpress.Utils;
using DevExpress.Images;
using DevExpress.Utils.Design;
using DevExpress.Xpf.Core;
using Jasmine.Core.Aspects;
using IDialogService=Prism.Services.Dialogs.IDialogService;
namespace Jasmine.Core.Chat.Views
{
    public class ChatViewModel : AsyncViewModelBase, IDisposable
    {        
        string _loggedUser;
        string _chatUser;
        readonly IChatService _chatService;
        readonly IEventAggregator _eventAggregator;
        private readonly Timer _typingTimer;

        public ChatViewModel(IEventAggregator eventAggregator, IDialogService dialogService, IAuthorizationCache authorizationCache,
            IChatService chatService) : base(eventAggregator, dialogService, authorizationCache)
        {            
            _eventAggregator = eventAggregator;
            _chatService = chatService;
            LoadMoreCommand = new AsyncCommand(ExecuteLoadMoreAsync);
            SendMessageCommand = new AsyncCommand(ExecuteSendMessage, CanSendMessage);
            TypingCommand = new AsyncCommand(ExecuteTypingAsync, CanExecuteTyping);
            IsActiveChanged += OnIsActiveChanged;
            _typingTimer = new Timer();

            chatService.ParticipantLoggedIn += ParticipantLogin;
            chatService.ParticipantLoggedOut += ParticipantDisconnection;
            chatService.ParticipantDisconnected += ParticipantDisconnection;
            chatService.ParticipantReconnected += ParticipantReconnection;
            chatService.ConnectionClosed += Disconnected;
            chatService.ConnectionOpened += Connected;            
            chatService.ParticipantTyping += ParticipantTyping;
            chatService.NotifyMessageStatuses += NotifyMessageStatuses;
            _eventAggregator.GetEvent<NewTextMessageEvent>().Subscribe(NewTextMessage);
        }

        private void NotifyMessageStatuses(string name, Guid[] messageIds, NotificationType notificationType)
        {
            if (name == _chatUser)
            {
                var chatMessage = ChatMessages.Where(i => messageIds.Contains(i.MessageId) && i.Author != "Date").ToList();
                chatMessage.ForEach(i => i.NotificationType = notificationType);
            }
        }

        private async void OnIsActiveChanged(object sender, EventArgs e)
        {
            if (IsActive)
            {
                if (Caption != null)
                {
                    _eventAggregator.GetEvent<ChatViewActivatedEvent>().Publish(Caption);
                    ClearNewMessagePanel();
                    await FillInComingChatAsync();                    
                }
            }
            else
            {
                _eventAggregator.GetEvent<ChatViewActivatedEvent>().Publish(null);
            }
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            _loggedUser = ClaimsPrincipal.Current.FindFirst("EmployeeName").Value;
            _chatUser = Caption;
            if (Caption != null)
            {
                _eventAggregator.GetEvent<ChatViewActivatedEvent>().Publish(Caption);
            }
            if (navigationContext.Parameters.TryGetValue("isConnected", out bool isConnected))
            {
                IsConnected = isConnected;
            }

            if (navigationContext.Parameters.TryGetValue("isParticipantLoggedIn", out bool isParticipantLoggedIn))
            {
                IsParticipantLoggedIn = isParticipantLoggedIn;
            }

            ParticipantTypingText = IsConnected && IsParticipantLoggedIn ? "Online" : "Offline";

            if (!IsLoaded)
            {
                await UpdateCachedMessagesPendingStatus();
                await FillChatAsync();
            }
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.TryGetValue("caption", out string caption))
            {
                if (Caption != caption)
                    return false;
            }
            return true;
        }

        protected bool IsLoaded { get; private set; }

        [ShowWaitIndicator]
        async Task FillChatAsync()
        {
            var chatMessages = await _chatService.GetMessagesAsync(_loggedUser, _chatUser, false);
            if(chatMessages.Count > 0)
            {
                var glyph = DXImageHelper.GetImageSource("Up", ImageSize.Size16x16);
                ChatMessages.Add(new ChatMessageModel { Author = "Date", Message = $"{chatMessages.First().Time:dddd, dd MMMM yyyy}", Glyph= glyph });
                await FillChatAsync(chatMessages, false);
            }            
            await FillInComingChatAsync();
            await FillOffilineChatAsync();
            IsLoaded = true;
        }

        private async Task UpdateCachedMessagesPendingStatus()
        {
            await _chatService.UpdateCachedMessagesPendingStatusAsync(_loggedUser, _chatUser);
        }

        private async Task FillOffilineChatAsync()
        {
            var chatMessages = await _chatService.GetOfflineMessagesAsync(_loggedUser, _chatUser);
            if (chatMessages?.Count > 0)
            {
                for (int i = 0; i < chatMessages.Count; i++)
                {
                    var chatMessage = chatMessages[i];
                    await _chatService.CacheMessageAsync(_loggedUser, _chatUser, _loggedUser, chatMessage.MessageId, chatMessage.Message, false, chatMessage.NotificationType);
                }

                if (!ChatMessages.Any(c => c.Author == "System" && c.Message == "New Messages"))
                    ChatMessages.Add(new ChatMessageModel { Author = "System", Message = "New Messages" });

                var dates = chatMessages.Select(i => i.Time.Date).Distinct().ToList();
                for (int i = 0; i < dates.Count; i++)
                {
                    var date = dates[i];
                    if (!ChatMessages.Any(c => c.Author == "Date" && c.Message == $"{date:dddd, dd MMMM yyyy}"))
                        ChatMessages.Add(new ChatMessageModel { Author = "Date", Message = $"{date:dddd, dd MMMM yyyy}" });
                    await FillChatAsync(chatMessages, true);
                }
            }
        }

        private async Task FillInComingChatAsync()
        {
            var chatMessages = await _chatService.GetMessagesAsync(_loggedUser, _chatUser, true);
            if (chatMessages.Count > 0)
            {
                ChatMessages.Add(new ChatMessageModel { Author = "System", Message = "New Messages" });

                var dates = chatMessages.Select(i => i.Time.Date).Distinct().ToList();
                for (int i = 0; i < dates.Count; i++)
                {
                    var date = dates[i];
                    if (!ChatMessages.Any(c => c.Author == "Date" && c.Message == $"{date:dddd, dd MMMM yyyy}"))
                        ChatMessages.Add(new ChatMessageModel { Author = "Date", Message = $"{date:dddd, dd MMMM yyyy}" });
                    await FillChatAsync(chatMessages, true);
                }
            }
        }
        private async Task FillChatAsync(List<ChatMessageModel> chatMessages, bool isInComing)
        {
            ChatMessageModel previousChatMessage = ChatMessages.LastOrDefault(j => j.Author != "System" && j.Author != "Date"); 
            for (int i = 0; i < chatMessages.Count; i++)
            {
                var chatMessage = chatMessages[i];
                if (chatMessage != null)
                {
                    chatMessage.IsOriginNative = chatMessage.Author == _loggedUser;                    
                    chatMessage.HasAnchor = previousChatMessage?.IsOriginNative != chatMessage.IsOriginNative;
                    ChatMessages.Add(chatMessage);
                    previousChatMessage = ChatMessages.LastOrDefault(j => j.Author != "System" && j.Author != "Date");
                }
            }
            
            if(isInComing)
            {
                var msgIds = chatMessages.Select(i => i.MessageId).ToArray();
                await _chatService.MarkMessagesAsReadAsync(_loggedUser, msgIds);
                await _chatService.AcknowledgeUnicastMessagesAsync(_chatUser, _loggedUser, msgIds, NotificationType.ReadMessage);
            }
            
        }

        public ObservableCollection<ChatMessageModel> ChatMessages { get; set; } = new ObservableCollection<ChatMessageModel>();

        public string ParticipantTypingText { get; set; }
        public string TextMessage { get; set; }

        public override bool KeepAlive => true;

        #region SendMessageCommand

        public AsyncCommand SendMessageCommand { get; set; }

        protected bool CanSendMessage() => IsConnected;

        private async Task ExecuteSendMessage()
        {
            if (!string.IsNullOrEmpty(TextMessage))
            {
                try
                {
                    ClearNewMessagePanel();

                    //Write to chat window
                    var hasAnchor = ChatMessages.Count == 0 || !ChatMessages.Last().IsOriginNative;
                    var chatMessage = new ChatMessageModel
                    {
                        Author = _loggedUser,
                        Message = TextMessage,
                        Time = DateTime.Now,
                        IsOriginNative = true,
                        HasAnchor = hasAnchor
                    };
                    if (!ChatMessages.Any(i => i.Author == "Date" && i.Message == $"{chatMessage.Time:dddd, dd MMMM yyyy}"))
                    {
                        ChatMessages.Add(new ChatMessageModel
                        {
                            Author = "Date",
                            Message = $"{chatMessage.Time:dddd, dd MMMM yyyy}"
                        });
                    }
                    ChatMessages.Add(chatMessage);

                    //Write to cache                    
                    await _chatService.CacheMessageAsync(_loggedUser, _loggedUser, _chatUser, chatMessage.MessageId, chatMessage.Message, false, NotificationType.Default);

                    //Clear chat text
                    TextMessage = string.Empty;

                    //Send Message                    
                    await _chatService.SendUnicastMessageAsync(_chatUser, chatMessage.MessageId, chatMessage.Message);
                }
                catch (Exception) { }
                finally
                {
                    
                }
            }
        }

        private void ClearNewMessagePanel()
        {
            var newMessagePanel = ChatMessages?.SingleOrDefault(i => i.Author == "System");
            if (newMessagePanel != null)
                ChatMessages.Remove(newMessagePanel);
        }

        #endregion


        #region LoadMoreCommand

        public AsyncCommand LoadMoreCommand { get; set; }

        private async Task ExecuteLoadMoreAsync()
        {
            var endDate = ChatMessages.Where(i => i.Author != "Date" && i.Author != "System").Min(i => i.Time).Date;
            var chatMessages = await _chatService.GetMessagesAsync(_loggedUser, _chatUser, false, endDate);
            if (chatMessages.Count > 0)
            {
                chatMessages = chatMessages.OrderByDescending(i => i.Id).ToList();
                ChatMessageModel previousChatMessage = null;// ChatMessages.FirstOrDefault(j => j.Author != "System" && j.Author != "Date");
                for (int i = 0; i < chatMessages.Count; i++)
                {
                    var chatMessage = chatMessages[i];
                    if (chatMessage != null)
                    {
                        chatMessage.IsOriginNative = chatMessage.Author == _loggedUser;
                        if(i == chatMessages.Count - 1)
                            chatMessage.HasAnchor = !previousChatMessage.HasAnchor;                        
                        ChatMessages.Insert(0,chatMessage);
                        if(previousChatMessage != null) previousChatMessage.HasAnchor = previousChatMessage?.IsOriginNative != chatMessage.IsOriginNative;
                        previousChatMessage = ChatMessages.FirstOrDefault(j => j.Author != "System" && j.Author != "Date");
                    }
                }
                ChatMessages.Where(i => i.Author == "Date" && i.Glyph != null).ToList().ForEach(i => i.Glyph = null);
                var glyph = DXImageHelper.GetImageSource("Up", ImageSize.Size16x16);
                ChatMessages.Insert(0, new ChatMessageModel { Author = "Date",
                    Message = $"{chatMessages.First().Time:dddd, dd MMMM yyyy}",
                    Glyph = glyph
                });
            }
        }

        #endregion

        #region TypingCommand

        public AsyncCommand TypingCommand { get; set; }
        public bool IsConnected { get; set; }
        public bool IsParticipantLoggedIn { get; set; }

        protected bool CanExecuteTyping() => IsConnected && IsParticipantLoggedIn;

        private async Task ExecuteTypingAsync()
        {
            try
            {
                await _chatService.TypingAsync(_chatUser);
            }
            catch (Exception) { }
        }

        #endregion

        async void NewTextMessage(TextMessage textMessage)
        {
            var sender = textMessage.Sender;
            var mt = textMessage.MessageType;
            var msgId = textMessage.MessageId;
            var msg = textMessage.Message;

            if (IsActive && sender == _chatUser)
            {
                if (mt == MessageType.Unicast)
                {
                    ClearTypingPanel();
                    var hasAnchor = ChatMessages.Count == 0 || ChatMessages.Last().IsOriginNative;
                    var chatMessage = new ChatMessageModel
                    {
                        MessageId = msgId,
                        Author = sender,
                        Message = msg,
                        Time = DateTime.Now,
                        HasAnchor = hasAnchor
                    };
                    //Write to chat window   
                    DispatcherService.Invoke(() =>
                    {
                        if (!ChatMessages.Any(i => i.Author == "Date" && i.Message == $"{chatMessage.Time:dddd, dd MMMM yyyy}"))
                        {
                            ChatMessages.Add(new ChatMessageModel
                            {
                                Author = "Date",
                                Message = $"{chatMessage.Time:dddd, dd MMMM yyyy}"
                            });
                        }
                        ChatMessages.Add(chatMessage);
                    });

                    //Write to cache
                    await _chatService.CacheMessageAsync(_loggedUser, sender, _loggedUser, msgId, msg, false, NotificationType.ReadMessage);

                    //Acknowledge
                    await _chatService.AcknowledgeUnicastMessagesAsync(sender, _loggedUser, new Guid[] { msgId }, NotificationType.ReadMessage);
                }
            }
        }        

        private void ParticipantTyping(string name)
        {
            if (name == _chatUser)
            {
                ParticipantTypingText = $"{name} is typing...";

                //DispatcherService.Invoke(() =>
                //{
                //    if (!ChatMessages.Any(i => i.Author == "Typing"))
                //        ChatMessages.Add(new ChatMessageModel { Author = "Typing", Message = $"{name} is typing...", Time = DateTime.Now });
                //});

                _typingTimer.Stop();
                _typingTimer.Elapsed -= typingTimer_Elapsed;
                _typingTimer.Interval = 1500;
                _typingTimer.AutoReset = false;
                _typingTimer.Elapsed += typingTimer_Elapsed;
                _typingTimer.Start();

            }
        }

        private void typingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ClearTypingPanel();
        }

        private void ClearTypingPanel()
        {
            DispatcherService.Invoke(() =>
            {
                ParticipantTypingText = IsConnected && IsParticipantLoggedIn ? "Online" : "Offline";
                //var typingPanel = ChatMessages.SingleOrDefault(i => i.Author == "Typing");
                //if (typingPanel != null)
                //    ChatMessages.Remove(typingPanel);
                _typingTimer.Stop();
                _typingTimer.Elapsed -= typingTimer_Elapsed;
            });
        }

        public void Dispose()
        {
            _chatService.ParticipantLoggedIn -= ParticipantLogin;
            _chatService.ParticipantLoggedOut -= ParticipantDisconnection;
            _chatService.ParticipantDisconnected -= ParticipantDisconnection;
            _chatService.ParticipantReconnected -= ParticipantReconnection;
            _chatService.ConnectionClosed -= Disconnected;
            _chatService.ConnectionOpened -= Connected;            
            _chatService.ParticipantTyping -= ParticipantTyping;            
            _chatService.NotifyMessageStatuses -= NotifyMessageStatuses;
        }
        void ParticipantLogin(UserModel participant)
        {
            IsParticipantLoggedIn = true;
            ParticipantTypingText = "Online";
        }
        void ParticipantDisconnection(string name)
        {
            IsParticipantLoggedIn = false;
            ParticipantTypingText = "Offline";
        }
        void ParticipantReconnection(string name)
        {
            IsParticipantLoggedIn = true;
            ParticipantTypingText = "Online";
        }
        void Disconnected()
        {
            IsConnected = false;
            ParticipantTypingText = "Offline";
        }
        void Connected()
        {
            IsConnected = true;
            ParticipantTypingText = string.Empty;
        }
    }
}
