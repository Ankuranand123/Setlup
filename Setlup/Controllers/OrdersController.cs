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
        public IActionResult InsertOrders([FromHeader]string userId, Orders Objorder)
        {
            try
            {
                var str = _usersOrderService.InsertOrders(userId, Objorder);
                if (str == "Inserted")
                {
                    return Ok(str);
                }
                else
                {
                    return BadRequest(str);
                }
            }catch (Exception ex)
            {
                return BadRequest("Exception");
            }
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

        [HttpPost]
        [Route("UpdateOrder")]
        public IActionResult UpdateOrder([FromHeader] string userId, Orders Objorder)
        {
            try
            {
                var str = _usersOrderService.UpdateOrder(userId, Objorder);
                if (str == "Updated")
                {
                    return Ok(str);
                }
                else
                {
                    return BadRequest(str);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Exception");
            }
        }


        [HttpPost]
        [Route("UpdateOrderStatus")]
        public IActionResult UpdateOrderStatus([FromHeader] string userId, Orders Objorder)
        {
            try
            {
                var str = _usersOrderService.UpdateOrderStatus(userId, Objorder);
               // if (str == "Inserted")
               // {
                    return Ok(str);
              //  }
             //   else
             //   {
                //    return BadRequest(str);
             //   }
            }
            catch (Exception ex)
            {
                return BadRequest("Exception");
            }
        }


        [HttpGet]
        [Route("GetInvoiceFormat")]
        public IActionResult GetInvoiceFormat([FromHeader] string userId, string OrderId, int OrderStatus)
        {

            try
            {
                var obj = _usersOrderService.GetInvoiceFormat(userId, OrderId, OrderStatus);
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

        [HttpPost]
        [Route("UpdateOrderStatusWithInvoice")]
        public IActionResult UpdateOrderStatusWithInvoice([FromHeader] string userId, Orders Objorder)
        {
            try
            {
                var str = _usersOrderService.UpdateOrderStatusWithInvoice(userId, Objorder);
                if (str == "Updated")
                {
                    return Ok(str);
                }
                else
                {
                    return BadRequest(str);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Exception");
            }
        }


    }
}
