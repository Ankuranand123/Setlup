using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Setlup.Services;
using Setlup.Models;
using Setlup.Utilities;

namespace Setlup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _usersOrderService;
        public OrdersController(IOrderService usersOrderService)
        {
            _usersOrderService = usersOrderService;
        }

        [HttpPost]
        [Route("InsertOrders")]
        public void InsertOrders([FromHeader]string userId, Orders Objorder)
        {
            _usersOrderService.InsertOrders(userId, Objorder);
        }

        [HttpPost]
        [Route("InsertInventory")]
        public IActionResult InsertInventory([FromHeader] string userId, InventoryList Objinventorylist)
        {
            try
            {

                string strretrun = _usersOrderService.InsertInventory(userId, Objinventorylist);
                if (strretrun == "Inserted")
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        //[HttpGet]
        //[Route("GetInventory")]
        //public InventoryList GetInventory([FromHeader] string userId)
        //{

        //    return _usersOrderService.GetInventoryList(userId);
        //}

        [HttpGet]
        [Route("GetInventory")]
        public IActionResult GetInventory([FromHeader] string userId)
        {
            try
            {
                var obj = _usersOrderService.GetInventoryList(userId);
                if(obj != null)
                {
                    return Ok(obj);
                }
                else
                {
                    return BadRequest();
                }
               
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            
        }

        [HttpPost]
        [Route("UpdateInventory")]
        public void UpdateInventory([FromHeader] string userId, Inventory ObjInventory)
        {
            _usersOrderService.UpdateInventoryItem(userId, ObjInventory);
        }


        [HttpGet]
        [Route("getItems")]
        public string getItems(string str)
        {

            return _usersOrderService.getItems(str);
        }

        [HttpGet]
        [Route("GetSearchItems")]
        public IActionResult GetSearchItems([FromHeader]string userId, string SupplierId,  string SearchValue)
        {

            try
            {
                var obj = _usersOrderService.GetSearchItems(userId,SupplierId,SearchValue);
                if (obj != null)
                {
                    return Ok(obj);
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return BadRequest();
            }


        }


    }
}
