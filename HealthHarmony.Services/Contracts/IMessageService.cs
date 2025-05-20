using HealthHarmony.Entities.DTOs.Message;
using HealthHarmony.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Services.Contracts
{
    public interface IMessageService
    {
        Task SaveMessageAsync(MessageCreateDTO messageDto);
        Task<List<Message>> GetMessagesAsync(Guid senderUserId, Guid receiverUserId);
        Task<List<Guid>> GetConversationUserIdsAsync(Guid userId);
        Task<List<User>> SearchUsersAsync(string keyword);

    }
}
