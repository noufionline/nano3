using Jasmine.Core.Chat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jasmine.Core.Contracts
{
    public interface IChatRepository
    {
        Task<List<ChatMessageModel>> GetOfflineMessagesAsync(string loggedUser, string chatUser);
        Task<List<ChatMessageModel>> GetMessagesAsync(List<Guid> msgIds);
        Task<bool> HasUnreadMessages(string loggedUser, string chatUser);
    }
}
