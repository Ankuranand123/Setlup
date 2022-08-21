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
        private readonly IMongoClient _mongoClient;
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
            _mongoClient = mongoClient;
        }

        public string InsertOrders(string userId, Orders Objorder)
        {
            try
            {
                //First Inserting the order in orders collection and then 
                int itemCount = 0;
                var uid = cryptingData.Decrypt(userId);
                foreach (var item in Objorder.Items)
                {
                    // item.ItemTotalPrice = item.ItemPrice * item.ItemQty;
                    // Objorder.ItemCount++;
                    itemCount++;
                }
                Objorder.ItemCount = itemCount;
                Objorder.CreatedBy = uid;
                Objorder.CreatedDate = DateTime.Now;
                Objorder.OrderStatus = 0;
                Objorder.InvoiceStatus = 0;
             //   Objorder.TotalOrderPrice = 0;
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
                return "Inserted";
            }catch (Exception ex)
            {
                return "Exception";
            }

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

        public string UpdateOrder(string userId, Orders objOrder)
        {
            try
            {

                var uid = cryptingData.Decrypt(userId);
                int itemCount = 0;
                var a =  IsUserCustomer(uid, objOrder);
                if (a == 1)
                {
                    foreach(var Item in objOrder.Items)
                    {
                        //  Item.ItemTotalPrice = Item.ItemPrice * Item.ItemQty;
                        itemCount++;
                    }
                    objOrder.ItemCount = itemCount;
                    FilterDefinition<Orders> updatefilter = Builders<Orders>.Filter.Eq(x => x.OrderId, objOrder.OrderId);
                    var update = Builders<Orders>.Update.Set(p => p.Items, objOrder.Items).Set(p => p.UpdatedDate, DateTime.Now).Set(p=>p.ItemCount, objOrder.ItemCount);
                    var options = new UpdateOptions { IsUpsert = true };
                    _orders.UpdateOne(updatefilter, update, options);
                    //
                    MessageText Objmessagetext = new MessageText();
                    Objmessagetext.CombinationId = objOrder.CombinationId;
                    Objmessagetext.Message = "";
                    Objmessagetext.Customer_or_Supplier = 1;
                    Objmessagetext.MessageType = 3;
                    Objmessagetext.OrderId = objOrder.OrderId;
                    Objmessagetext.CreatedBy = uid;
                    Objmessagetext.CreatedDate = DateTime.Now;
                    Objmessagetext.Status = 1;
                    _MessageText.InsertOne(Objmessagetext);

                    return "Updated";
                }
                else
                {
                    return "Order can only be updated by customer";
                }
            }
            catch(Exception ex)
            {
                return "Exception";
            }

        }

        public int IsUserCustomer(string uid, Orders objOrder)
        {
           // var uid = cryptingData.Decrypt(userId);
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

                    if ((customlist[0].UserId == uid && customlist[0].Customer_or_Supplier == 2) || (customlist[0].Customer_SupplierId == uid && customlist[0].Customer_or_Supplier == 1))
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

        public string UpdateOrderStatus(string userId, Orders objOrder)
        {
            //Accept 1 Reject 2 Delivered 3
            var uid = cryptingData.Decrypt(userId);
            MessageText Objmessagetext = new MessageText();

            //First verify the user who is updating status is  supplier or not
            int ret =  IsUserSupplier(userId, objOrder);
            if(ret == 1)
            {
                //update status
                FilterDefinition<Orders> updatefilter = Builders<Orders>.Filter.Eq(x => x.OrderId, objOrder.OrderId);
                var update = Builders<Orders>.Update.Set(p => p.OrderStatus, objOrder.OrderStatus);
                var options = new UpdateOptions { IsUpsert = true };
                _orders.UpdateOne(updatefilter, update, options);

               
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
                Objmessagetext.MessageType = 4;
                Objmessagetext.OrderId = objOrder.OrderId;
                Objmessagetext.CreatedBy = uid;
                Objmessagetext.CreatedDate = DateTime.Now;
                Objmessagetext.Status = 1;
                _MessageText.InsertOne(Objmessagetext);

             //  return Objmessagetext.Message;

            }

            return Objmessagetext.Message;


        }

        //public void UpdateRevisedOrder(string userId, Orders objOrder)
        //{
        //    //First verify the user who is updating status is  supplier or not
        //    int ret = IsUserSupplier(userId, objOrder);
        //    if(ret == 1)
        //    {
        //        FilterDefinition<Orders> updatefilter = Builders<Orders>.Filter.Eq(x => x.OrderId, objOrder.OrderId);
        //        var update = Builders<Orders>.Update.Set(p => p.OrderStatus, objOrder.OrderStatus).Set(p => p.RevisedOrder, objOrder.RevisedOrder);
        //        var options = new UpdateOptions { IsUpsert = true };
        //        _orders.UpdateOne(updatefilter, update, options);
        //    }
          
        //}


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

        public string UpdateOrderStatusWithInvoice(string UserId, Orders objOrder)
        {
            //Accept 1 Reject 2 Delivered 3
            MessageText Objmessagetext = new MessageText();
            var uid = cryptingData.Decrypt(UserId);
            int totalOrderPrice = 0;
            int itemsCount = 0;

            FilterDefinition<Orders> updatefilter = Builders<Orders>.Filter.Eq(x => x.OrderId, objOrder.OrderId);
            var update = Builders<Orders>.Update.Set(p => p.OrderStatus, objOrder.OrderStatus);
            var options = new UpdateOptions { IsUpsert = true };
            _orders.UpdateOne(updatefilter, update, options);

            if (objOrder.OrderStatus == 1)
            {
                //var updateInvoceStatus = Builders<Orders>.Update.Set(p => p.InvoiceStatus, 1).Set(p => p.InvoiceDate, DateTime.Now);
                //var options1 = new UpdateOptions { IsUpsert = true };


                foreach (var item in objOrder.Items)
                {
                    itemsCount++;

                    item.ItemTotalPrice = (item.ItemPrice * item.ItemQty) - item.Discount;
                    totalOrderPrice = totalOrderPrice + item.ItemTotalPrice;
                }

                var update2 = Builders<Orders>.Update.Set(p => p.Items, objOrder.Items).Set(p => p.InvoiceNo, objOrder.InvoiceNo).Set(p => p.TotalOrderPrice, totalOrderPrice)
                    .Set(p => p.ItemCount, itemsCount).Set(p => p.InvoiceStatus, 1).Set(p => p.InvoiceDate, DateTime.Now)
                    .Set(p=>p.IsTax,objOrder.IsTax).Set(p=>p.Amount,objOrder.Amount);
                var options2 = new UpdateOptions { IsUpsert = true };
                _orders.UpdateOne(updatefilter, update2, options2);
                Objmessagetext.CombinationId = objOrder.CombinationId;
                // Objmessagetext.Message = "";
                Objmessagetext.Customer_or_Supplier = 2;
                Objmessagetext.Message = "Order has been accepted";
                Objmessagetext.MessageType = 4; // OrderStatus
                Objmessagetext.OrderId = objOrder.OrderId;
                Objmessagetext.CreatedBy = uid;
                Objmessagetext.CreatedDate = DateTime.Now;
                Objmessagetext.Status = 1;
                _MessageText.InsertOne(Objmessagetext);
            }

            // Objmessagetext.CombinationId = objOrder.CombinationId;
            //// Objmessagetext.Message = "";
            // Objmessagetext.Customer_or_Supplier = 2;
            // if (objOrder.OrderStatus == 1)
            // {
            //     Objmessagetext.Message = "Order has been accepted";
            // }
            // else if (objOrder.OrderStatus == 2)
            // {
            //     Objmessagetext.Message = "Order has been Rejected";
            // }
            // else
            // {
            //     Objmessagetext.Message = "Order has been Delivered";
            // }
            // Objmessagetext.MessageType = 4; // OrderStatus
            // Objmessagetext.OrderId = objOrder.OrderId;
            // Objmessagetext.CreatedBy = uid;
            // Objmessagetext.CreatedDate = DateTime.Now;
            // Objmessagetext.Status = 1;
            // _MessageText.InsertOne(Objmessagetext);

            return "Updated";
        }


        public Orders GetInvoiceFormat(string UserId,string OrderId,int OrderStatus)
        {
            Orders ObjOrder = new Orders();
            if(OrderStatus == 1)
            {
                FilterDefinition<Orders> filter = Builders<Orders>.Filter.Eq(x => x.OrderId, OrderId);
             var OrderDetails =  _orders.Find(filter).FirstOrDefault();
                ObjOrder.ItemCount = OrderDetails.ItemCount;
                ObjOrder.Items = OrderDetails.Items;
                ObjOrder.InvoiceNo = "SET001-01";
                ObjOrder.InvoiceDate = DateTime.Now;
                ObjOrder.IsDue = 1;
                ObjOrder.Amount = 1000;

            }

            return ObjOrder;

        }
    }
}
