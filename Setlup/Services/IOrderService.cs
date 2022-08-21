using Setlup.Models;

namespace Setlup.Services
{
    public interface IOrderService
    {
        string InsertOrders(string userId,Orders Objorder);

        string InsertInventory (string userId,InventoryList Objinventorylist);

        string UpdateOrder(string userId, Orders Objorder);

        string UpdateOrderStatus(string userId, Orders ObjOrder);

        InventoryList GetInventoryList(string UserId);

        void UpdateInventoryItem(string UserId,Inventory objInventory);

        string getItems(string str);

        InventoryList GetSearchItems(string UserId, string SupplierId, string SearchValue);

        
         Orders GetInvoiceFormat(string UserId, string OrderId, int OrderStatus);

        string UpdateOrderStatusWithInvoice(string UserId, Orders objOrder);
    }
}
