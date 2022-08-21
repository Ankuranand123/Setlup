using Setlup.Models;

namespace Setlup.Services
{
    public interface IusersAddService
    {
        UserRetDetails InsertUser(userMobileDetails mobileDetails);

        string InsertUserBusinessTypeDetails(string userId,userDetails userDetails);

        string GetDetails(string id);

        string InserUserDetails(string userId,userDetails userDetails);

        string InsertCustomerSupplier(string userId, Users_CustomerSuppliers objCustomerSuppliers);

        userDetails GetUserDetails(string Userid);

        Customer_SuppliersList GetCustomerSuppliers(string userId);

        BusinessTypeDetails GetBusinessType();

        string InsertStates(States_list ObjStates);

        States_list GetStatesList();
    }
}
