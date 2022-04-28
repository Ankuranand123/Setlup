using MongoDB.Bson;
using MongoDB.Driver;
using Setlup.Models;
using Setlup.Utilities;



namespace Setlup.Services
{
    public class OrderService : IOrderService
    {

        private readonly IMongoCollection<userMobileDetails> _userMobileDetails;
        private readonly IMongoCollection<userDetails> _userDetails;
        private readonly IMongoCollection<Users_CustomerSuppliers> _userCustomerSuppliers;
        private readonly IMongoCollection<Orders> _orders;
        private readonly IMongoCollection<Inventory> _Inventory;
        private readonly IMongoCollection<MessageText> _MessageText;
        //private readonly IMongoDatabase _database;

        public OrderService(ISetlupStoreDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _userMobileDetails = database.GetCollection<userMobileDetails>(settings.StudentCoursesCollectionName);
            _userDetails = database.GetCollection<userDetails>(settings.userDetailsCollectionName);
            _userCustomerSuppliers = database.GetCollection<Users_CustomerSuppliers>(settings.UserCustomerSuppliersCollectionName);
            _orders = database.GetCollection<Orders>(settings.OrdersCollectionName);
            _Inventory = database.GetCollection<Inventory>(settings.InventoryCollectionName);
            _MessageText = database.GetCollection<MessageText>(settings.MessageTextCollectionName);

        }

        public void InsertOrders(string userId, Orders Objorder)
        {

            //First Inserting the order in orders collection and then 
            var uid = cryptingData.Decrypt(userId);
            Objorder.CreatedBy = uid;
            Objorder.CreatedDate = DateTime.Now;
            _orders.InsertOne(Objorder);
            MessageText Objmessagetext = new MessageText();
            Objmessagetext.CombinationId = Objorder.CombinationId;
            Objmessagetext.Message = "";
            Objmessagetext.Customer_or_Supplier = 1;
            Objmessagetext.MessageType = 2;
            Objmessagetext.OrderId = Objorder.OrderId;
            Objmessagetext.CreatedBy = uid;
            Objmessagetext.CreatedDate = DateTime.Now;
            Objmessagetext.Status = 1;
            _MessageText.InsertOne(Objmessagetext);

        }

        public string InsertInventory(string userId, InventoryList Objinventorylist)
        {
            try
            {
                var uid = cryptingData.Decrypt(userId);

                foreach (var ObjInventory in Objinventorylist.InvenotryItems)
                {
                    ObjInventory.UserID = uid;
                    ObjInventory.CreatedDate = DateTime.Now;
                    ObjInventory.Status = 1;

                }
                _Inventory.InsertMany(Objinventorylist.InvenotryItems);

                return "Inserted";
            }
            catch(Exception ex)
            {
                return "Error";
            }

        }

        public void UpdateOrder(string userId, Orders objOrder)
        {
            FilterDefinition<Orders> updatefilter = Builders<Orders>.Filter.Eq(x => x.OrderId, objOrder.OrderId);
            var update = Builders<Orders>.Update.Set(p => p.OrderStatus, objOrder.OrderStatus).Set(p => p.InvoiceDetails, objOrder.InvoiceDetails).Set(p => p.InvoiceStatus, objOrder
                .InvoiceStatus);
            var options = new UpdateOptions { IsUpsert = true };
           
            _orders.UpdateOne(updatefilter, update, options);

        }

        public void UpdateOrderStatus(string userId, Orders objOrder)
        {
            //Accept 1 Reject 2 Delivered 3
            var uid = cryptingData.Decrypt(userId);

            //First verify the user who is updating status is  supplier or not
            int ret =  IsUserSupplier(userId, objOrder);
            if(ret == 1)
            {
                //update status
                FilterDefinition<Orders> updatefilter = Builders<Orders>.Filter.Eq(x => x.OrderId, objOrder.OrderId);
                var update = Builders<Orders>.Update.Set(p => p.OrderStatus, objOrder.OrderStatus);
                var options = new UpdateOptions { IsUpsert = true };
                _orders.UpdateOne(updatefilter, update, options);

                MessageText Objmessagetext = new MessageText();
                Objmessagetext.CombinationId = objOrder.CombinationId;
                if(objOrder.OrderStatus == 1)
                {
                    Objmessagetext.Message = "Order has been accepted";
                }else if(objOrder.OrderStatus == 2)
                {
                    Objmessagetext.Message = "Order has been Rejected";
                }
                else
                {
                    Objmessagetext.Message = "Order has been Delivered";
                }
                Objmessagetext.Message = "";
                Objmessagetext.Customer_or_Supplier = 2;
                Objmessagetext.MessageType = 3;
                Objmessagetext.OrderId = objOrder.OrderId;
                Objmessagetext.CreatedBy = uid;
                Objmessagetext.CreatedDate = DateTime.Now;
                Objmessagetext.Status = 1;
                _MessageText.InsertOne(Objmessagetext);

            }




        }

        public void UpdateRevisedOrder(string userId, Orders objOrder)
        {
            //First verify the user who is updating status is  supplier or not
            int ret = IsUserSupplier(userId, objOrder);
            if(ret == 1)
            {
                FilterDefinition<Orders> updatefilter = Builders<Orders>.Filter.Eq(x => x.OrderId, objOrder.OrderId);
                var update = Builders<Orders>.Update.Set(p => p.OrderStatus, objOrder.OrderStatus).Set(p => p.RevisedOrder, objOrder.RevisedOrder);
                var options = new UpdateOptions { IsUpsert = true };
                _orders.UpdateOne(updatefilter, update, options);
            }
          
        }


        public int IsUserSupplier(string userId, Orders objOrder)
        {
            var uid = cryptingData.Decrypt(userId);
            //First need to verify if the user who is updating the status of order is supplier for this order  or not.

            FilterDefinition<Orders> filter = Builders<Orders>.Filter.Eq(x => x.OrderId, objOrder.OrderId);
            var orderlist = _orders.Find(filter).ToList();
            if (orderlist.Count > 0)
            {
                var Com_uniqueId = orderlist[0].CombinationId;
                FilterDefinition<Users_CustomerSuppliers> Customfilter = Builders<Users_CustomerSuppliers>.Filter.Eq(x => x.CustomerSuppliersId, Com_uniqueId);
                var customlist = _userCustomerSuppliers.Find(Customfilter).ToList();
                if (customlist.Count > 0)
                {

                    if ((customlist[0].UserId == uid && customlist[0].Customer_or_Supplier == 1) || (customlist[0].Customer_SupplierId == uid && customlist[0].Customer_or_Supplier == 2))
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }

        public void InsertMessage(string userId,MessageText objmessagetext)
        {
            var uid = cryptingData.Decrypt(userId);
            objmessagetext.MessageType = 1;
            objmessagetext.CreatedBy = uid;
            objmessagetext.CreatedDate = DateTime.Now;
            if(objmessagetext.Customer_or_Supplier == 1)
            {
                objmessagetext.Customer_or_Supplier = 2;
            }
            else
            {
                objmessagetext.Customer_or_Supplier = 1;
            }

            _MessageText.InsertOne(objmessagetext);

        }

        //Fetch Inventory for user

        public InventoryList GetInventoryList(string UserId)
        {
            InventoryList inventoryList = new InventoryList();
            try
            {
                var uid = cryptingData.Decrypt(UserId);
                var filter = Builders<Inventory>.Filter.Where(x => x.UserID == uid && x.Status == 1);
                var li = _Inventory.Find(filter).ToList();
                inventoryList.InvenotryItems = li;
                return inventoryList;
            }
            catch(Exception ex)
            {
                inventoryList = null;
                return inventoryList;
            }

        }

       public void UpdateInventoryItem(string UserId, Inventory objInventory)
        {
            var uid = cryptingData.Decrypt(UserId);
            var filter = Builders<Inventory>.Filter.Where(x=>x.InventoryId == objInventory.InventoryId && x.UserID == uid);
            var update = Builders<Inventory>.Update.Set(p => p.ItemName, objInventory.ItemName).Set(p => p.Price, objInventory.Price)
                .Set(p => p.Quantity, objInventory.Price).Set(p => p.Size, objInventory.Size);
            var options = new UpdateOptions { IsUpsert = true };
            _Inventory.UpdateOne(filter,update,options);


        }

        public string getItems(string str)
        {
            //  var collection = database.GetCollection<BsonDocument>("<<name of the collection>>");

            //  var filter = Builders<Inventory>.Filter.Regex("Items", new BsonRegularExpression(str));
            var filter = Builders<Inventory>.Filter.Where(x => x.ItemName.ToLower().Contains(str));

            // var result = await collection.Find(filter).ToListAsync();

            var li = _Inventory.Find(filter).ToList();
            return li[0].ItemName;


        }


        public InventoryList GetSearchItems(string UserId, string SupplierId,string SearchValue)
        {
            InventoryList objinventorylist = new InventoryList();
            try
            {
              //  var uid = cryptingData.Decrypt(UserId);
                var filter = Builders<Inventory>.Filter.Where(x => x.UserID == SupplierId && x.ItemName.ToLower().Contains(SearchValue) && x.Status == 1);
                var li = _Inventory.Find(filter).ToList();
                objinventorylist.InvenotryItems = li;
                return objinventorylist;
                var a = 1;
            }catch (Exception ex)
            {
                objinventorylist = null;
                return objinventorylist;
            }


        }
    }
}
