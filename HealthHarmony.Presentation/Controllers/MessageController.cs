using HealthHarmony.Entities.DTOs.Message;
using HealthHarmony.Entities.Models;
using HealthHarmony.Hubs;
using HealthHarmony.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthHarmony.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IHubContext<MessageHub> _messageHubContext;

        public MessageController(IMessageService messageService, IHubContext<MessageHub> messageHubContext)
        {
            _messageService = messageService;
            _messageHubContext = messageHubContext;
        }

        // Yeni mesaj gönderme endpoint'i
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageCreateDTO messageDto)
        {
            try
            {
                // Mesajı veritabanına kaydet
                var senderUserId = messageDto.SenderUserId ?? Guid.Empty;
                var receiverUserId = messageDto.ReceiverUserId ?? Guid.Empty;

                if (senderUserId == Guid.Empty || receiverUserId == Guid.Empty)
                {
                    return BadRequest("Geçersiz kullanıcı ID'si.");
                }

                var message = new Message
                {
                    SenderUserId = senderUserId,
                    ReceiverUserId = receiverUserId,
                    MessageContent = messageDto.MessageContent,
                    SentTime = DateTime.Now,
                    IsRead = false
                };

                // Veritabanına kaydet
                await _messageService.SaveMessageAsync(messageDto);

                // Mesajı alıcıya ilet
                await _messageHubContext.Clients.User(receiverUserId.ToString())
                    .SendAsync("ReceiveMessage", messageDto.MessageContent);

                return Ok(new { message = "Mesaj başarıyla gönderildi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Mesaj gönderilirken hata oluştu.", details = ex.Message });
            }
        }

        // Kullanıcılar arasında eski mesajları almak için endpoint
        [HttpGet("history")]
        public async Task<IActionResult> GetMessageHistory([FromQuery] Guid receiverUserId)
        {
            try
            {
                var senderUserId = Guid.Parse(User.Identity.Name);  // JWT'den kullanıcı ID'sini alıyoruz

                // Eski mesajları al
                var messages = await _messageService.GetMessagesAsync(senderUserId, receiverUserId);

                // Eski mesajları alıcıya gönder
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Mesajlar alınırken hata oluştu.", details = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string keyword)
        {
            try
            {
                var results = await _messageService.SearchUsersAsync(keyword);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Kullanıcı arama hatası", details = ex.Message });
            }
        }
        [HttpGet("conversations")]
        public async Task<IActionResult> GetConversations([FromQuery] Guid userId)
        {
            try
            {
                var userIds = await _messageService.GetConversationUserIdsAsync(userId);
                return Ok(userIds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Hata oluştu", details = ex.Message });
            }
        }
    }
}
