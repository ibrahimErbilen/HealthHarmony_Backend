using HealthHarmony.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Repositories.Contracts
{
    public interface IMessageRepository
    {
        Task AddMessageAsync(Message message);
        Task<List<Message>> GetMessagesByUserIdsAsync(Guid senderUserId, Guid receiverUserId);
        public Task<List<Guid>> GetConversationUserIdsAsync(Guid userId);
        public Task<List<User>> SearchUsersAsync(string keyword);

    }
}
