using Setlup.Models;

namespace Setlup.Services
{
    public interface IOrderService
    {
        void InsertOrders(string userId,Orders Objorder);

        string InsertInventory (string userId,InventoryList Objinventorylist);

        void UpdateOrder(string userId, Orders Objorder);

        void UpdateOrderStatus(string userId, Orders ObjOrder);

        InventoryList GetInventoryList(string UserId);

        void UpdateInventoryItem(string UserId,Inventory objInventory);

        string getItems(string str);

        InventoryList GetSearchItems(string UserId, string SupplierId, string SearchValue);
    }
}
