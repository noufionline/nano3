using Jasmine.Core.Chat.Model;
using Jasmine.Core.Contracts;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static PostSharp.Patterns.Diagnostics.FormattedMessageBuilder;    
namespace Jasmine.Core.Chat
{
    public class ChatService : IChatService
    {
        HubConnection _connection;
        string _loggedUserName;
        readonly IApiTokenProvider _apiTokenProvider;
        readonly IChatRepository _repository;
        LogSource _logger;
        public event Action<UserModel> ParticipantLoggedIn;
        public event Action<string> ParticipantLoggedOut;
        public event Action ConnectionClosed;
        public event Action ConnectionOpened;
        public event Action LoggingOff;
        public event Action<string> ParticipantDisconnected;
        public event Action<string> ParticipantReconnected;
        public event Action<string, Guid, string, MessageType> NewTextMessage;
        public event Action<string> ParticipantTyping;        
        public event Action<string, Guid[], NotificationType> NotifyMessageStatuses;
        public event Action<UserModel> PhotoUpdated;

        public ChatService(IApiTokenProvider apiTokenProvider, IChatRepository repository)
        {
            _repository = repository;
            _apiTokenProvider = apiTokenProvider;
            _logger = LogSource.Get();         
        }

        public string GetBaseAddress()
        {
            string enviorment = ConfigurationManager.AppSettings.Get("Environment");
            if (enviorment == "Production")
            {
                return ConfigurationManager.AppSettings.Get("HttpBaseAddress");

            }
            return ConfigurationManager.AppSettings.Get("LocalHttpBaseAddress");
        }

        public async Task ConnectAsync()
        {

            try
            {
                if (_connection == null)
                {
                    //var url = "https://localhost:5051/api/chat";
                    var url = $"{GetBaseAddress()}chat";
                    _connection = new HubConnectionBuilder()
                                .WithUrl(url, options =>
                                {
                                    options.AccessTokenProvider = () => _apiTokenProvider.GetTokenAsync();
                                    //options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
                                })
                                .ConfigureLogging(logging =>
                                {
                                    // Log to the Output Window
                                    logging.AddDebug();

                                    // This will set ALL logging to Debug level
                                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                                })
                                .Build();
                    _connection.On<UserModel>("ParticipantLogin", (user) => ParticipantLoggedIn?.Invoke(user));
                    _connection.On<string>("ParticipantLogout", (n) => ParticipantLoggedOut?.Invoke(n));
                    _connection.On<string>("ParticipantDisconnection", (n) => ParticipantDisconnected?.Invoke(n));
                    _connection.On<string>("ParticipantReconnection", (n) => ParticipantReconnected?.Invoke(n));
                    _connection.On<string, Guid, string>("UnicastTextMessage", (n, id, m) => NewTextMessage?.Invoke(n, id, m, MessageType.Unicast));
                    _connection.On<string>("ParticipantTyping", (p) => ParticipantTyping?.Invoke(p));
                    //_connection.On<string, Guid, NotificationType>("NotifyMessageStatus", (r,id, nt) => NotifyMessageStatus?.Invoke(r,id, nt));
                    _connection.On<string, Guid[], NotificationType>("NotifyMessageStatuses", (r, ids, nt) => NotifyMessageStatuses?.Invoke(r, ids, nt));
                    _connection.On<UserModel>("PhotoUpdated", (user) => PhotoUpdated?.Invoke(user));

                    _connection.Closed += DisconnectedAsync;
                    ServicePointManager.DefaultConnectionLimit = 10;
                }
                if (_connection.State == HubConnectionState.Disconnected) await _connection.StartAsync();
                if (_connection.State == HubConnectionState.Connected)
                {
                    Debug.WriteLine("Connected");
                    ConnectionOpened?.Invoke();
                }
            }
            catch (Exception)
            {

            }
        }

        async Task DisconnectedAsync(Exception arg)
        {
            await Task.Run(() =>
            {
                ConnectionClosed?.Invoke();
            });
        }

        public async Task<List<UserModel>> LoginAsync(string loggedUserName, byte[] photo)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                ConnectionOpened?.Invoke();
                _loggedUserName = loggedUserName;
                return await _connection.InvokeAsync<List<UserModel>>("Login", loggedUserName, photo);
            }
            return null;
        }

        public async Task SendPhotoUpdateAsync(string loggedUserName, byte[] photo)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                ConnectionOpened?.Invoke();
                _loggedUserName = loggedUserName;
                await _connection.InvokeAsync("PhotoUpdated", loggedUserName, photo);
            }
        }

        public async Task LogoutAsync()
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                LoggingOff?.Invoke();
                await _connection?.InvokeAsync("Logout", _loggedUserName);
            }
        }

        public async Task SendUnicastMessageAsync(string recepient, Guid msgId, string msg)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("UnicastTextMessage", _loggedUserName, recepient, msgId, msg);
            }
        }

        public async Task TypingAsync(string recepient)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("Typing", _loggedUserName, recepient);
            }
        }        

        public async Task AcknowledgeUnicastMessagesAsync(string sender, string receiver, Guid[] msgIds, NotificationType notificationType)
        {
            if (_connection != null && _connection.State == HubConnectionState.Connected)
            {
                await _connection.InvokeAsync("AcknowledgeUnicastMessages", sender, receiver, msgIds, notificationType);
            }
        }

        public string GetChatDirectory()
        {
            var appPath = Environment.GetEnvironmentVariable("LocalAppData");
            var chatDirectory = Path.Combine(appPath, "chat");
            if (!Directory.Exists(chatDirectory)) Directory.CreateDirectory(chatDirectory);
            return chatDirectory;
        }
        public async Task<List<ParticipantModel>> GetCachedUsersAsync(string userName)
        {
            using (var connection = GetConnection(userName))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Users";
                var reader = await command.ExecuteReaderAsync();
                var users = new List<ParticipantModel>();
                while (await reader.ReadAsync())
                {
                    var name = reader["Name"];
                    var photo = reader["Photo"];
                    var user = new ParticipantModel();
                    user.Name = (string)name;
                    if (photo != DBNull.Value)
                        user.Photo = (byte[])photo;

                    users.Add(user);
                }
                return users;
            }
        }

        public async Task CacheUsersAsync(string userName, List<UserModel> users)
        {
            var cachedUsers = await GetCachedUsersAsync(userName);
            using (var connection = GetConnection(userName))
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (cachedUsers.Any(c => c.Name.Contains(users[i].Name)))
                    {
                        var command = connection.CreateCommand();
                        command.CommandText = "UPDATE Users SET Photo = @photo WHERE Name = @name";
                        command.Parameters.Add(new SQLiteParameter("@photo", DbType.Binary) { Value = users[i].Photo });
                        command.Parameters.AddWithValue("@name", users[i].Name);
                        var result = await command.ExecuteNonQueryAsync();
                    }
                    else
                    {
                        var command = connection.CreateCommand();
                        if (users[i].Photo != null)
                        {
                            command.CommandText = "INSERT INTO Users (Name, Photo) VALUES (@name, @photo)";
                            command.Parameters.AddWithValue("@name", users[i].Name);
                            command.Parameters.Add(new SQLiteParameter("@photo", DbType.Binary) { Value = users[i].Photo });
                        }
                        else
                        {
                            command.CommandText = "INSERT INTO Users (Name) VALUES (@name)";
                            command.Parameters.AddWithValue("@name", users[i].Name);
                        }
                        var result = await command.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        public async Task CacheMessageAsync(string db, string author, string receiver, Guid msgId, string msg, bool isNew, NotificationType notificationType)
        {
            using (var connection = GetConnection(db))
            {
                var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO ChatMessages (MessageId, Author, Receiver, Message, MessageTime, Status, IsNew)
                                                           VALUES(@messageId, @author, @receiver, @message, @time, @status, @isNew)";
                //command.Parameters.Add(new SQLiteParameter("@messageId", DbType.Binary) { Value = msgId.ToByteArray() });                
                command.Parameters.AddWithValue("@messageId", msgId.ToString());
                command.Parameters.AddWithValue("@author", author);
                command.Parameters.AddWithValue("@receiver", receiver);
                command.Parameters.AddWithValue("@message", msg);
                command.Parameters.AddWithValue("@time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@status", notificationType);
                command.Parameters.AddWithValue("@isNew", isNew);
                var result = await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<ChatMessageModel>> GetMessagesAsync(string loggedUser, string chatUser, bool isNew, DateTime? endDate = null)
        {
            using (var connection = GetConnection(loggedUser))
            {
                var command = connection.CreateCommand();
                if (!isNew)
                {
                    if (endDate == null)
                    {
                        command.CommandText = @"WITH chats as(SELECT Id, MessageId, Author, Receiver, Message, MessageTime, date(MessageTime) mt, Status, IsNew 
                                    FROM ChatMessages), maxdate as(SELECT date(MAX(MessageTime)) mt FROM ChatMessages )
                                    SELECT chats.* from chats INNER JOIN maxdate on chats.mt = maxdate.mt
                                    WHERE ((Author = @loggedUser AND Receiver = @chatUser) OR (Author = @chatUser AND Receiver = @loggedUser)) AND IsNew = @isNew;";
                    }
                    else
                    {
                        command.CommandText = @"WITH chats as(SELECT Id, MessageId, Author, Receiver, Message, MessageTime, date(MessageTime) mt, Status, IsNew 
                                    FROM ChatMessages WHERE MessageTime < @endDate), maxdate as(SELECT date(MAX(MessageTime)) mt FROM ChatMessages WHERE MessageTime < @endDate)
                                    SELECT chats.* from chats INNER JOIN maxdate on chats.mt = maxdate.mt
                                    WHERE ((Author = @loggedUser AND Receiver = @chatUser) OR (Author = @chatUser AND Receiver = @loggedUser)) AND IsNew = @isNew;";
                    }
                }
                else
                {
                    command.CommandText = @"WITH chats as(SELECT Id, MessageId, Author, Receiver, Message, MessageTime, date(MessageTime) mt, Status, IsNew 
                                    FROM ChatMessages), maxdate as(SELECT date(MAX(MessageTime)) mt FROM ChatMessages )
                                    SELECT chats.* from chats INNER JOIN maxdate on chats.mt = maxdate.mt
                                    WHERE Author = @chatUser AND Receiver = @loggedUser AND IsNew = @isNew;";
                }
                command.Parameters.AddWithValue("@loggedUser", loggedUser);
                command.Parameters.AddWithValue("@chatUser", chatUser);
                command.Parameters.AddWithValue("@isNew", isNew);
                if (endDate != null)
                    command.Parameters.AddWithValue("@endDate", endDate);                
                var reader = await command.ExecuteReaderAsync();
                var chatMessages = new List<ChatMessageModel>();
                while (await reader.ReadAsync())
                {
                    var id = reader["Id"];
                    var msgId = reader["MessageId"];
                    var author = reader["Author"];
                    var message = reader["Message"];
                    var time = reader["MessageTime"];
                    var status = reader["Status"];
                    var chatMessage = new ChatMessageModel()
                    {
                        Id = int.Parse(id.ToString()),
                        MessageId = Guid.Parse(msgId.ToString()),
                        Author = author.ToString(),
                        Message = message.ToString(),
                        Time = DateTime.Parse(time.ToString()),
                        NotificationType = (NotificationType)int.Parse((status.ToString()))
                    };

                    chatMessages.Add(chatMessage);
                }
                return chatMessages;
            }
        }

        private string CommandAsSql_Text(SQLiteCommand command)
        {
            string query = command.CommandText;

            foreach (SQLiteParameter p in command.Parameters)
                query = Regex.Replace(query,
                   "\\B" + p.ParameterName + "\\b", p.Value.ToString());

            return query;
        }

        public async Task MarkMessagesAsReadAsync(string db, Guid[] msgIds)
        {
            using (var connection = GetConnection(db))
            {
                var command = connection.CreateCommand();
                var param = new List<string>();
                foreach (var msgId in msgIds)
                {
                    param.Add($"'{msgId.ToString()}'");
                }
                command.CommandText = $"UPDATE ChatMessages SET IsNew = 0, Status = 3 WHERE MessageId IN({string.Join(",", param)})";
                var result = await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateMessageStatusesAsync(string db, Guid[] msgIds, NotificationType notificationType)
        {
            using (var connection = GetConnection(db))
            {
                var command = connection.CreateCommand();
                var param = new List<string>();
                foreach (var msgId in msgIds)
                {
                    param.Add($"'{msgId.ToString()}'");
                }
                command.CommandText = $"UPDATE ChatMessages SET Status = @status WHERE MessageId IN({string.Join(",", param)})";
                command.Parameters.AddWithValue("@status", notificationType);
                var result = await command.ExecuteNonQueryAsync();
            }
        }

        private SQLiteConnection GetConnection(string userName)
        {

            var chatDirectory = GetChatDirectory();
#if DEBUG
            chatDirectory = Path.Combine(chatDirectory, "Debug");
            if (!Directory.Exists(chatDirectory)) Directory.CreateDirectory(chatDirectory);
#endif
            var db = Path.Combine(chatDirectory, $"{userName.Replace(" ", "")}.sqlite");
            if (!File.Exists(db)) CreateDatabase(db);
            CreateUserTableAsync(db);
            CreateChatMessagesTable(db);
            var dbConnection = new SQLiteConnection($"Data Source={db};Version=3;");
            dbConnection.Open();
            return dbConnection;
        }

        private void CreateDatabase(string db)
        {
            SQLiteConnection.CreateFile(db);           
        }

        private void CreateUserTableAsync(string db)
        {
            var dbConnection = new SQLiteConnection($"Data Source={db};Version=3;");
            try
            {
                
                dbConnection.Open();
                var command = dbConnection.CreateCommand();
                command.CommandText = @" CREATE TABLE IF NOT EXISTS ""Users"" (
                                        ""Id""	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, 
                                        ""Name""	TEXT NOT NULL UNIQUE,
                                        ""Photo""	BLOB );";
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.Info.Write(Formatted($"DB : ({db}) -{ex.Message}"));
                throw;
            }
            finally
            {
                dbConnection.Dispose();
            }

        }

        private void CreateChatMessagesTable(string db)
        {
            var dbConnection = new SQLiteConnection($"Data Source={db};Version=3;");
            try
            {
                dbConnection.Open();
                var command = dbConnection.CreateCommand();
                command.CommandText = @" CREATE TABLE IF NOT EXISTS ""ChatMessages"" (
                                    ""Id""	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, 
                                    ""MessageId""	TEXT NOT NULL,
                                    ""Author""	TEXT NOT NULL,
                                    ""Receiver""	TEXT NOT NULL,
                                    ""Message""	TEXT NOT NULL,
                                    ""MessageTime""	TEXT NOT NULL,
                                    ""Status""	NUMERIC DEFAULT 0,
                                    ""IsNew""	NUMERIC DEFAULT 0);";
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.Info.Write(Formatted($"DB : ({db}) -{ex.Message}"));
                throw;
            }
            finally
            {
                dbConnection.Dispose();
            }
        }

        public Task<List<ChatMessageModel>> GetOfflineMessagesAsync(string loggedUser, string chatUser)
        {
            return _repository.GetOfflineMessagesAsync(loggedUser, chatUser);
        }
        public async Task UpdateCachedMessagesPendingStatusAsync(string loggedUser, string chatUser)
        {
            var msgIds = new List<Guid>();
            using (var connection = GetConnection(loggedUser))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT MessageId FROM ChatMessages WHERE Author = @loggedUser AND Receiver = @chatUser AND Status <> 3";
                command.Parameters.AddWithValue("@loggedUser", loggedUser);
                command.Parameters.AddWithValue("@chatUser", chatUser);

                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var id = reader["MessageId"];
                    if (id != null)
                        msgIds.Add(Guid.Parse(id.ToString()));
                }
            }
            if (msgIds.Count > 0)
            {
                var messages = await _repository.GetMessagesAsync(msgIds);
                if (messages != null)
                {
                    for (int i = 0; i < messages.Count; i++)
                    {
                        var message = messages[i];
                        using (var connection = GetConnection(loggedUser))
                        {
                            var command = connection.CreateCommand();
                            command.CommandText = $"UPDATE ChatMessages SET Status = {(int)message.NotificationType} WHERE MessageId = '{message.MessageId}'";
                            var result = await command.ExecuteNonQueryAsync();
                        }
                    }
                }
            }


        }
        public Task<bool> HasUnreadMessages(string loggedUser, string chatUser)
        {
            return _repository.HasUnreadMessages(loggedUser, chatUser);
        }



    }

    public enum MessageType
    {
        Broadcast,
        Unicast
    }

    public enum NotificationType
    {
        Default,
        Received,
        Delivered,
        ReadMessage
    }
}
