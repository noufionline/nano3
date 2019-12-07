using Jasmine.Core.Chat.Model;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jasmine.Core.Chat
{
    public interface IChatService
    {
        Task SendPhotoUpdateAsync(string loggedUserName, byte[] photo);

        event Action<string> ParticipantReconnected;
        event Action<string> ParticipantDisconnected;
        event Action ConnectionClosed;
        event Action ConnectionOpened;
        event Action<string> ParticipantLoggedOut;
        event Action<UserModel> ParticipantLoggedIn;
        event Action LoggingOff;
        event Action<string, Guid, string, MessageType> NewTextMessage;
        event Action<string> ParticipantTyping;
        
        event Action<string, Guid[], NotificationType> NotifyMessageStatuses;
        event Action<UserModel> PhotoUpdated;

        Task ConnectAsync();
        Task<List<UserModel>> LoginAsync(string loggedUserName, byte[] photo);
        Task LogoutAsync();
        Task SendUnicastMessageAsync(string recepient, Guid msgId, string msg);
        Task TypingAsync(string recepient);
        //Task AcknowledgeUnicastMessageAsync(string sender, string reciever, Guid msgId, NotificationType notificationType);
        Task AcknowledgeUnicastMessagesAsync(string sender, string reciever, Guid[] msgIds, NotificationType notificationType);
        string GetChatDirectory();
        Task<List<ParticipantModel>> GetCachedUsersAsync(string userName);
        Task CacheUsersAsync(string userName, List<UserModel> users);
        Task CacheMessageAsync(string db, string author, string receiver, Guid msgId, string msg, bool isNew, NotificationType notificationType);
        Task<List<ChatMessageModel>> GetMessagesAsync(string loggedUser, string chatUser, bool isNew, DateTime? endDate = null);
        Task MarkMessagesAsReadAsync(string db, Guid[] msgIds);
        Task UpdateMessageStatusesAsync(string db, Guid[] msgIds, NotificationType notificationType);
        Task<List<ChatMessageModel>> GetOfflineMessagesAsync(string loggedUser, string chatUser);
        Task UpdateCachedMessagesPendingStatusAsync(string loggedUser, string chatUser);
        Task<bool> HasUnreadMessages(string loggedUser, string chatUser);
    }
}
