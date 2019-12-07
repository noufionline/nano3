using Jasmine.Core.Chat.Model;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Patterns.Model;
using System.Collections.ObjectModel;
using System.Security.Claims;
using Jasmine.Core.Mvvm;
using DevExpress.Mvvm;
using System.IO;
using Jasmine.Core.Contracts;
using System.Timers;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using PostSharp.Patterns.Xaml;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using Prism;
using Prism.Ioc;
using Prism.Events;
using Jasmine.Core.Chat.Events;
using Jasmine.Core.Chat.Extensions;
using Jasmine.Core.Common;
using Jasmine.Core.Notification;
using Jasmine.Core.Notification.Views;

namespace Jasmine.Core.Chat.Views
{
    [NotifyPropertyChanged]
    public class LoggedUserCollectionViewModel : DxMvvmServicesBase, INavigationAware
    {

        TaskFactory ctxTaskFactory;
        readonly IChatService _chatService;
        string _userName;
        int _userId;
        readonly IUserProfileManager _profileManager;
        readonly IRegionManager _regionManager;
        readonly IContainerExtension _container;
        readonly IEventAggregator _eventAggregator;
        IMessageBoxService MessageBoxService => GetService<IMessageBoxService>();
        IDispatcherService DispatcherService => GetService<IDispatcherService>();
        public INotificationService DefaultNotificationService => GetService<INotificationService>();

        public LoggedUserCollectionViewModel(IChatService chatService,
            IUserProfileManager profileManager,
            IRegionManager regionManager, IContainerExtension container,
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _container = container;
            _regionManager = regionManager;
            _profileManager = profileManager;

            _userName = ClaimsPrincipal.Current.FindFirst("EmployeeName").Value;
            _userId = Convert.ToInt32(ClaimsPrincipal.Current.FindFirst("EmployeeId").Value);
            _chatService = chatService;
            ctxTaskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
            chatService.ParticipantLoggedIn += ParticipantLogin;
            chatService.ParticipantLoggedOut += ParticipantDisconnection;
            chatService.ParticipantDisconnected += ParticipantDisconnection;
            chatService.ParticipantReconnected += ParticipantReconnection;
            chatService.ConnectionClosed += Disconnected;
            chatService.LoggingOff += LoggingOff;
            chatService.NewTextMessage += NewTextMessage;
            chatService.NotifyMessageStatuses += NotifyMessageStatuses;
            chatService.PhotoUpdated += PhotoUpdated;
            RefreshCommand = new AsyncCommand(ExecuteRefreshAsync, CanExecuteRefresh);
            //Todo - Remove the unread message notification when chat view is activated
            _eventAggregator.GetEvent<ChatViewActivatedEvent>().Subscribe((participant) =>
            {
                var activeParticipant = Participants.SingleOrDefault(i => i.Name == participant);
                if(activeParticipant != null) activeParticipant.HasUnreadMessage = false;
                ActiveParticipant = activeParticipant;

            });
        }

        private void LoggingOff() => DispatcherService.Invoke(() =>
        {
            _chatService.ParticipantLoggedIn -= ParticipantLogin;
            _chatService.ParticipantLoggedOut -= ParticipantDisconnection;
            _chatService.ParticipantDisconnected -= ParticipantDisconnection;
            _chatService.ParticipantReconnected -= ParticipantReconnection;
            _chatService.ConnectionClosed -= Disconnected;
            _chatService.LoggingOff -= LoggingOff;
            _chatService.NewTextMessage -= NewTextMessage;
            _chatService.NotifyMessageStatuses -= NotifyMessageStatuses;
            Participants.Clear();
        });

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            await InitializeAsync();
        }
        public bool IsNavigationTarget(NavigationContext navigationContext) => true;


        void ReorderParticipants()
        {
            DispatcherService.Invoke(() =>
            {
                Participants = Participants.OrderByDescending(i => i.HasUnreadMessage).ThenByDescending(i => i.IsLoggedIn).ToObservableList();
            });
        }
        //Dispatcher is required while re-using the method from events
        async Task InitializeAsync()
        {
            try
            {
                var connected = await Connect();
                if (connected)
                {
                    var cachedUsers = await _chatService.GetCachedUsersAsync(_userName);
                    DispatcherService.Invoke(() =>
                    {
                        for (int i = 0; i < cachedUsers.Count; i++)
                        {
                            var cachedUser = cachedUsers[i];
                            if (!Participants.Any(p => p.Name.Contains(cachedUser.Name)))
                                Participants.Add(cachedUser);
                        }
                    });

                    var participants = await Login();
                    if (participants != null && participants.Count > 0)
                    {
                        await _chatService.CacheUsersAsync(_userName, participants);
                        DispatcherService.Invoke(() =>
                        {
                            for (int i = 0; i < participants.Count; i++)
                            {
                                var participant = Participants.SingleOrDefault(p => p.Name == participants[i].Name);
                                if (participant != null)
                                {
                                    participant.IsLoggedIn = true;
                                    participant.Photo = participants[i].Photo;
                                }
                                else
                                {
                                    Participants.Add(new ParticipantModel
                                    {
                                        Name = participants[i].Name,
                                        Photo = participants[i].Photo,
                                        IsLoggedIn = true
                                    });
                                }
                            }                            
                        });
                    }

                    for (int i = 0; i < Participants.Count; i++)
                    {
                        var participant = Participants[i];
                        var hasUnreadMessages = await _chatService.HasUnreadMessages(_userName, participant.Name);
                        if (hasUnreadMessages)
                        {
                            DispatcherService.Invoke(() =>
                            {
                                participant.HasUnreadMessage = true;                                
                            });
                        }
                    }
                    ReorderParticipants();
                    IsLoggedIn = true;
                }
            }
            catch (Exception)
            {

            }
        }

        private async Task<bool> Connect()
        {
            try
            {
                await _chatService.ConnectAsync();
                IsConnected = true;
                return true;
            }
            catch (Exception) { return false; }
        }


        private async Task<List<UserModel>> Login()
        {
            try
            {
                ProfilePic = await _profileManager.GetProfilePhotoAsync(_userId);
                var users = await _chatService.LoginAsync(_userName, ProfilePic);
                return users;
            }
            catch (Exception) { return null; }
        }

        public bool IsConnected { get; set; }

        public bool IsLoggedIn { get; set; }

        public byte[] ProfilePic { get; set; }

        public ObservableCollection<ParticipantModel> Participants { get; set; } = new ObservableCollection<ParticipantModel>();

        void ParticipantLogin(UserModel user)
        {
            UpdateUser(user);
        }        

        void PhotoUpdated(UserModel user)
        {
            UpdateUser(user);
        }
        private void UpdateUser(UserModel user)
        {
            if (user.Name != _userName)
            {
                _chatService.CacheUsersAsync(_userName, new List<UserModel> { user });
                var ptp = Participants.FirstOrDefault(p => string.Equals(p.Name, user.Name));
                if (IsLoggedIn)
                {
                    if (ptp == null)
                    {
                        DispatcherService.Invoke(() =>
                        Participants.Add(new ParticipantModel
                        {
                            Name = user.Name,
                            Photo = user.Photo,
                            IsLoggedIn = user.IsLoggedIn
                        }));
                    }
                    else
                    {
                        DispatcherService.Invoke(() => { ptp.IsLoggedIn = true; ptp.Photo = user.Photo; });
                    }
                }
                ReorderParticipants();
            }
        }

        private void ParticipantDisconnection(string name)
        {
            DispatcherService.Invoke(() =>
            {
                var person = Participants.Where((p) => string.Equals(p.Name, name)).FirstOrDefault();
                if (person != null)
                    person.IsLoggedIn = false;
            });
            ReorderParticipants();
        }

        private void ParticipantReconnection(string name)
        {
            DispatcherService.Invoke(() =>
            {
                var person = Participants.Where((p) => string.Equals(p.Name, name)).FirstOrDefault();
                if (person != null)
                    person.IsLoggedIn = true;
            });
            ReorderParticipants();
        }

        private async void Disconnected()
        {
            IsConnected = false;
            IsLoggedIn = false;

            DispatcherService.Invoke(() =>
            {
                foreach (var participant in Participants)
                {
                    participant.IsLoggedIn = false;
                }
            });
            do
            {
                await Reconnect();
                await Task.Delay(10000);
            } while (!IsConnected && !IsLoggedIn);
            ReorderParticipants();
        }


        private async Task Reconnect()
        {
            await InitializeAsync();
        }


        #region RefreshCommand

        public AsyncCommand RefreshCommand { get; set; }

        protected bool CanExecuteRefresh() => IsConnected;

        private async Task ExecuteRefreshAsync()
        {
            await InitializeAsync();
        }




        #endregion

        #region ChatCommand

        [Command]
        public ICommand ChatCommand { get; set; }

        private void ExecuteChat(ParticipantModel participant)
        {
            var parameters = new NavigationParameters();

            var caption = participant.Name;
            parameters.Add("caption", caption);

            if (participant.Photo != null)
            {
                var converter = new ImageSourceConverter();
                var captionImage = (BitmapSource)converter.ConvertFrom(participant.Photo);
                parameters.Add("captionimage", captionImage);
            }

            parameters.Add("isConnected", IsConnected);
            parameters.Add("isParticipantLoggedIn", participant.IsLoggedIn);

            var view = caption.Replace(" ", string.Empty);

            _container.RegisterForNavigation<ChatView>(view);
            participant.HasUnreadMessage = false;
            _regionManager.RequestNavigate(KnownRegions.DocumentRegion, view, parameters);
            ReorderParticipants();
        }

        #endregion
        public ParticipantModel ActiveParticipant { get; set; }

        //Todo - Add unread messages to the notification        
        async void NewTextMessage(string sender, Guid msgId, string msg, MessageType mt)
        {
            if (ActiveParticipant == null || ActiveParticipant.Name != sender)
            {
                if (mt == MessageType.Unicast)
                {
                    if (DefaultNotificationService != null)
                    {
                        DispatcherService.Invoke(() =>
                        {
                            INotification notificationService = DefaultNotificationService.CreateCustomNotification(
                                    new { Type = sender, Message = msg });
                            notificationService.ShowAsync();

                            //var viewName = sender.Replace(" ", "");
                            //var notification = new NotificationInfo() {Name = sender, Description = msg };
                            //_container.RegisterForNavigation<NotificationView>(viewName);
                            //_regionManager.RequestNavigate(KnownRegions.NotificationRegion, viewName,
                            //new NavigationParameters { { "notificationInfo", notification } });
                        });
                    }

                    DispatcherService.Invoke(() =>
                    {
                        var participant = Participants.SingleOrDefault(i => i.Name == sender);
                        if (participant != null)
                            participant.HasUnreadMessage = true;
                    });

                    await _chatService.CacheMessageAsync(_userName, sender, _userName, msgId, msg, true, NotificationType.Delivered);

                    await _chatService.AcknowledgeUnicastMessagesAsync(sender, _userName, new Guid[] { msgId }, NotificationType.Delivered);
                    
                }
            }
            else
            {
                if (mt == MessageType.Unicast)
                {
                    _eventAggregator.GetEvent<NewTextMessageEvent>().Publish(new TextMessage { Sender = sender, MessageId = msgId, Message = msg, MessageType = mt });
                }
            }
            ReorderParticipants();
        }

        private async void NotifyMessageStatuses(string name, Guid[] messageIds, NotificationType notificationType)
        {
            await _chatService.UpdateMessageStatusesAsync(_userName, messageIds, notificationType);
        }
        
    }
}
