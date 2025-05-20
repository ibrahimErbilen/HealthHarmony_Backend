using HealthHarmony.Entities.DTOs.Message;
using HealthHarmony.Services.Contracts;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace HealthHarmony.Hubs
{
    public class MessageHub : Hub
    {
        private readonly IMessageService _messageService;

        public MessageHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // Mesajı alır ve belirtilen kullanıcıya iletir
        public async Task SendMessage(string receiverId, string message)
        {
            var senderUserId = Guid.Parse(Context.UserIdentifier);  // Kullanıcı ID'si (JWT üzerinden alınabilir)
            var receiverUserId = Guid.Parse(receiverId);  // Alıcı kullanıcı ID'si

            // Yeni mesajı kaydediyoruz
            var messageDto = new MessageCreateDTO
            {
                SenderUserId = senderUserId,
                ReceiverUserId = receiverUserId,
                MessageContent = message,
                SentTime = DateTime.Now
            };

            await _messageService.SaveMessageAsync(messageDto);

            // Alıcıya mesajı iletiyoruz
            await Clients.User(receiverId).SendAsync("ReceiveMessage", message);
        }

        // Eski mesajları alır
        public async Task GetMessages(string receiverId)
        {
            var senderUserId = Guid.Parse(Context.UserIdentifier);  // Kullanıcı ID'si
            var receiverUserId = Guid.Parse(receiverId);  // Alıcı kullanıcı ID'si

            // Veritabanından eski mesajları alıyoruz
            var messages = await _messageService.GetMessagesAsync(senderUserId, receiverUserId);

            // Kullanıcıya eski mesajları gönderiyoruz
            await Clients.Caller.SendAsync("ReceiveOldMessages", messages);
        }
    }
}
