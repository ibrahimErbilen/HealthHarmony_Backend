using HealthHarmony.Entities.DTOs.Message;
using HealthHarmony.Entities.Models;
using HealthHarmony.Hubs;
using HealthHarmony.Repositories.Contracts;
using HealthHarmony.Services.Contracts;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Services.Concreate
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        // Yeni mesajı kaydeder
        public async Task SaveMessageAsync(MessageCreateDTO messageDto)
        {
            var message = new Message
            {
                SenderUserId = messageDto.SenderUserId,
                ReceiverUserId = messageDto.ReceiverUserId,
                MessageContent = messageDto.MessageContent,
                SentTime = messageDto.SentTime,
                IsRead = false
            };

            await _messageRepository.AddMessageAsync(message);
        }

        // Eski mesajları alır
        public async Task<List<Message>> GetMessagesAsync(Guid senderUserId, Guid receiverUserId)
        {
            return await _messageRepository.GetMessagesByUserIdsAsync(senderUserId, receiverUserId);
        }

        public async Task<List<Guid>> GetConversationUserIdsAsync(Guid userId)
        {
            return await _messageRepository.GetConversationUserIdsAsync(userId);
        }
        public async Task<List<User>> SearchUsersAsync(string keyword)
        {
            return await _messageRepository.SearchUsersAsync(keyword);
        }
    }
}
