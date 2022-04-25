using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Setlup.Services;
using Setlup.Models;
using Setlup.Utilities;

namespace Setlup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {

        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;

        }

        [HttpPost]
        [Route("InsertMessage")]
        public void InsertMessage([FromHeader] string userId, MessageText ObjMessageText)
        {
            _messageService.InsertTextMessage(userId, ObjMessageText);
        }
    }
}
