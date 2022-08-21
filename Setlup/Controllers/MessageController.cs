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
        public IActionResult InsertMessage([FromHeader] string userId, MessageText ObjMessageText)
        {
            try
            {

             var str =  _messageService.InsertTextMessage(userId, ObjMessageText);
                if(str == "Exception")
                {
                    return BadRequest("");
                }
                else
                {
                    return Ok("Inserted");
                }
            }catch (Exception ex)
            {
                return BadRequest("");
            }
        }


        [HttpGet]
        [Route("GetChat")]
        public IActionResult GetChat([FromHeader] string userId, string CombinationId, int PageIndex)
        {
            try
            {
                var Obj = _messageService.GetChat(userId, CombinationId, PageIndex);
                if(Obj == null)
                {
                    return BadRequest("Error");
                }else
                return Ok(Obj);
            }
            catch (Exception ex)
            {
                return BadRequest("Exception");
            }

        }

        [HttpGet]
        [Route("GetRequiredIds")]
        public IActionResult GetRequiredIds([FromHeader] string userId, string CombinationId)
        {
            try
            {
                var Obj = _messageService.GetRequiredIds(userId, CombinationId);
                if (Obj == null)
                {
                    return BadRequest("Error");
                }
                else
                    return Ok(Obj);
            }
            catch (Exception ex)
            {
                return BadRequest("Exception");
            }

        }
    }
}
