using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Setlup.Services;
using Setlup.Models;
using Setlup.Utilities;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace Setlup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase 
    {

        private readonly IusersAddService _usersAddService;
        public UsersController(IusersAddService usersAddService)
        {
            _usersAddService = usersAddService;
        }

        //[HttpPost]
        //[Route("insertuser")]
        //public string insertuser([FromBody] userMobileDetails mobileDetails)
        //{
        //   var s1 =  _usersAddService.InsertUser(mobileDetails);
        //    return s1;
        //This is commented

        //}

        [HttpPost]
        [Route("insertuser")]
        public IActionResult insertuser([FromBody] userMobileDetails mobileDetails)
        {
            try
            {
                var s1 = _usersAddService.InsertUser(mobileDetails);
                if(s1 == "Exception")
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(s1);
                }
               
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
         

        }

        //[HttpPost]
        //[Route("insertuserBusinessType")]
        //public string insertuserBusinessType([FromHeader]string userId,[FromBody] userDetails userDetails)
        //{
        //   string s1 = _usersAddService.InsertUserBusinessTypeDetails(userId,userDetails);
        //    return s1;

        //}

        [HttpPost]
        [Route("insertuserBusinessType")]
        public IActionResult insertuserBusinessType([FromHeader] string userId, [FromBody] userDetails userDetails)
        {
            try
            {
                string s1 = _usersAddService.InsertUserBusinessTypeDetails(userId, userDetails);
                string s2 = "";
                if(s1=="Exception")
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(s1);
                }
             
            }catch(Exception ex)
            {
                return BadRequest();
            }

        }

        [HttpGet]
        [Route("getMobile")]
        public string getMobile(string id)
        {
           var s1=  _usersAddService.GetDetails(id);
            return s1;
        }

        //[HttpPost]
        //[Route("InserUserDetails")]
        //public string InserUserDetails([FromHeader] string userId, [FromBody] userDetails userDetails)
        //{
        //   var s1 =  _usersAddService.InserUserDetails(userId, userDetails);
        //     return s1;

        //}

        [HttpPost]
        [Route("InserUserDetails")]
        public IActionResult InserUserDetails([FromHeader] string userId, [FromBody] userDetails userDetails)
        {
            try
            {
                var s1 = _usersAddService.InserUserDetails(userId, userDetails);
                // return new ContentResult() { Content = s1, StatusCode = 201 };
                if (s1 == "Exception")
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(s1);
                }
            }catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("getEncryptedString")]
        public string getEncryptedString(string s12)
        {

            string s1 = cryptingData.Encrypt(s12);
            return s1;

        }


        [HttpPost]
        [Route("InsertCustomerSupplier")]
        public IActionResult InsertCustomerSupplier([FromHeader] string userId, [FromBody] Users_CustomerSuppliers objCustomerSuppliers)
        {
            try
            {

                var s1 = _usersAddService.InsertCustomerSupplier(userId, objCustomerSuppliers);
                if(s1 == "Exception")
                {

                    return BadRequest();
                }else
                {
                    return Ok(s1);
                }
               
            }catch (Exception ex)
            {
                return BadRequest();
            }
            // return s1;

        }

        [HttpGet]
        [Route("GetUserDetails")]
        public userDetails GetUserDetails([FromHeader] string userId)
        {

          return  _usersAddService.GetUserDetails(userId);
        }

        [HttpGet]
        [Route("GetCustomersSuppliersList")]
        public Customer_SuppliersList GetCustomersSuppliersList([FromHeader] string userId)
        {

            return _usersAddService.GetCustomerSuppliers(userId);
        }

        //[HttpGet]
        //[Route("GetBusinessType")]
        //public BusinessTypeDetails GetBusinessType()
        //{
        //    var s1 = _usersAddService.GetBusinessType();
        //    return s1;
        //}

        [HttpGet]
        [Route("GetBusinessType")]
        public IActionResult GetBusinessType()
        {
            try
            {
                var s1 = _usersAddService.GetBusinessType();
                if(s1 == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(s1);
                }
             
               
            }catch (Exception ex)
            {
                return BadRequest();
            }
        }



    }
}
