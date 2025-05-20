using HealthHarmony.Entities.Models;
using HealthHarmony.Repositories.Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Repositories.Concreate
{
    public class MessageRepository : IMessageRepository
    {
        private readonly string _connectionString;

        public MessageRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        // Mesaj eklemek
        public async Task AddMessageAsync(Message message)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("usp_AddMessage", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@SenderUserId", message.SenderUserId);
                cmd.Parameters.AddWithValue("@ReceiverUserId", message.ReceiverUserId);
                cmd.Parameters.AddWithValue("@MessageContent", message.MessageContent);
                cmd.Parameters.AddWithValue("@SentTime", message.SentTime);
                cmd.Parameters.AddWithValue("@IsRead", message.IsRead);

                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<Guid>> GetConversationUserIdsAsync(Guid userId)
        {
            var userIds = new List<Guid>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetConversationUserIds", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("ConversationUserId")))
                            {
                                userIds.Add(reader.GetGuid(reader.GetOrdinal("ConversationUserId")));
                            }
                        }
                    }
                }
            }

            return userIds;
        }

        // Kullanıcılar arasındaki eski mesajları alır
        public async Task<List<Message>> GetMessagesByUserIdsAsync(Guid senderUserId, Guid receiverUserId)
        {
            var messages = new List<Message>();

            using (var conn = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("usp_GetMessages", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@SenderUserId", senderUserId);
                cmd.Parameters.AddWithValue("@ReceiverUserId", receiverUserId);

                conn.Open();
                var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    var message = new Message
                    {
                        MessageId = reader.GetInt32(reader.GetOrdinal("MessageId")),
                        SenderUserId = reader.GetGuid(reader.GetOrdinal("SenderUserId")),
                        ReceiverUserId = reader.GetGuid(reader.GetOrdinal("ReceiverUserId")),
                        MessageContent = reader.GetString(reader.GetOrdinal("MessageContent")),
                        SentTime = reader.GetDateTime(reader.GetOrdinal("SentTime")),
                        IsRead = reader.GetBoolean(reader.GetOrdinal("IsRead"))
                    };
                    messages.Add(message);
                }
            }

            return messages;
        }

        public async Task<List<User>> SearchUsersAsync(string keyword)
        {
            var users = new List<User>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_SearchUsers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Keyword", keyword);

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            users.Add(new User
                            {
                                UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                                Username = reader.GetString(reader.GetOrdinal("UserName")),
                                Email = reader.GetString(reader.GetOrdinal("Email"))
                                // Diğer alanlar varsa ekleyebilirsin
                            });
                        }
                    }
                }
            }

            return users;
        }
    }
}
