using Jasmine.Core.Chat.Model;
using Jasmine.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jasmine.Core.Repositories
{
    public class ChatRepository :RestApiRepositoryBase, IChatRepository
    {
        public Task<List<ChatMessageModel>> GetOfflineMessagesAsync(string loggedUser, string chatUser)
        {
            string request = $"chats/offline/{loggedUser}/{chatUser}";
            return ReadAllAsStreamAsync<ChatMessageModel>(request);
        }

        public async Task<List<ChatMessageModel>> GetMessagesAsync(List<Guid> msgIds)
        {            
            string request = $"chats";
            return await QueryWithPostAndReadWithStreamsAsync<List<ChatMessageModel>,List<Guid>>(request, msgIds);
        }
        public Task<bool> HasUnreadMessages(string loggedUser, string chatUser)
        {
            string request = $"chats/unread/{loggedUser}/{chatUser}";
            return ReadAsAsync<bool>(request);
        }

        public ChatRepository(IHttpClientFactory factory) : base(factory)
        {
        }
    }
}
