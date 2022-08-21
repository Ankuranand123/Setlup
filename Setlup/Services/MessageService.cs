using MongoDB.Bson;
using MongoDB.Driver;
using Setlup.Models;
using Setlup.Utilities;


namespace Setlup.Services
{
    public class MessageService : IMessageService
    {

        private readonly IMongoCollection<userMobileDetails> _userMobileDetails;
        private readonly IMongoCollection<userDetails> _userDetails;
        private readonly IMongoCollection<Users_CustomerSuppliers> _userCustomerSuppliers;
        private readonly IMongoCollection<Orders> _orders;
        private readonly IMongoCollection<Inventory> _Inventory;
        private readonly IMongoCollection<MessageText> _MessageText;
        //private readonly IMongoDatabase _database;

        public MessageService(ISetlupStoreDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _userMobileDetails = database.GetCollection<userMobileDetails>(settings.StudentCoursesCollectionName);
            _userDetails = database.GetCollection<userDetails>(settings.userDetailsCollectionName);
            _userCustomerSuppliers = database.GetCollection<Users_CustomerSuppliers>(settings.UserCustomerSuppliersCollectionName);
            _orders = database.GetCollection<Orders>(settings.OrdersCollectionName);
            _Inventory = database.GetCollection<Inventory>(settings.InventoryCollectionName);
            _MessageText = database.GetCollection<MessageText>(settings.MessageTextCollectionName);

        }

        public  string InsertTextMessage(string UserId, MessageText ObjMessageText)
        {
            try
            {
                //If message is a simple text message then message  type is 1
                var uid = cryptingData.Decrypt(UserId);
                //If user has clicked on Customer then Customer_Supplier type is 1 but since its supplier who is typing message so Customer_Supplier typ should be 2
                if (ObjMessageText.Customer_or_Supplier == 1)
                {
                    ObjMessageText.Customer_or_Supplier = 2;
                }
                else
                {
                    ObjMessageText.Customer_or_Supplier = 1;
                }
                ObjMessageText.CreatedBy = uid;
                ObjMessageText.MessageType = 1;
                ObjMessageText.CreatedDate = DateTime.Now;
                ObjMessageText.OrderId = ObjectId.GenerateNewId().ToString();
                ObjMessageText.Status = 1;
                _MessageText.InsertOne(ObjMessageText);
                return ObjMessageText.Message;
            }
            catch (Exception ex)
            {
                return "Exception";
            }

        }

        public MessageTextList GetChat(string UserId,string CombinationId,int PageIndex)
        {
            try
            {

                int SkipRecords = PageIndex * 20;  // initialy it will be 0
                MessageTextList ObjMessageTextList = new MessageTextList();
                //if (PageIndex == 0)
                //{
                //    Details ObjDetails = GetSupplierIdFromCombinationId(CombinationId);
                //    ObjMessageTextList.SupplierId = ObjDetails.SupplierID;
                //    ObjMessageTextList.CustomerId = ObjDetails.CustomerID;
                //    ObjMessageTextList.CombinationId = ObjDetails.CombinationID;
                //}
                var sort = Builders<MessageText>.Sort.Descending("CreatedDate");

                var filter = Builders<MessageText>.Filter.Where(x => x.CombinationId == CombinationId && x.Status == 1);

                //  ObjMessageTextList.ObjmsgtextList = _MessageText.Find(filter).Sort(sort).Skip(SkipRecords).Limit(20).ToList();
               List<MessageText> Objmessagetext =  _MessageText.Find(filter).Sort(sort).Skip(SkipRecords).Limit(20).ToList();

                foreach(var message in Objmessagetext)
                {
                    if(message.MessageType == 2 || message.MessageType == 3 || message.MessageType == 4)
                    {
                        FilterDefinition<Orders> filterorder = Builders<Orders>.Filter.Eq(x => x.OrderId, message.OrderId);

                       var order = _orders.Find(filterorder).FirstOrDefault();
                       message.ItemList = order.Items;
                        message.OrderStatus = order.OrderStatus;
                        message.InvoiceDate = order.InvoiceDate;
                        message.ItemsCount = order.ItemCount;
                        message.IsDue = order.IsDue;
                        message.Amount = order.Amount;
                        message.InvoiceNo = order.InvoiceNo;
                    }
                  


                }

                ObjMessageTextList.ObjmsgtextList = Objmessagetext;


                return ObjMessageTextList;
            }catch (Exception ex)
            {
                MessageTextList Objmsglist = new MessageTextList();
                Objmsglist = null;
                return Objmsglist;
            }


        }

        public Details GetSupplierIdFromCombinationId(string CombinationId)
        {
            Details ObjDetails = new Details();
            string SupplierId = "";
            var filter = Builders<Users_CustomerSuppliers>.Filter.Where(x => x.CustomerSuppliersId == CombinationId);
            var ObjRecord = _userCustomerSuppliers.Find(filter).FirstOrDefault();
            if (ObjRecord != null)
            {
                if (ObjRecord.Customer_or_Supplier == 2)
                {
                    ObjDetails.SupplierID = ObjRecord.Customer_SupplierId;
                    ObjDetails.CombinationID = CombinationId;
                    ObjDetails.CustomerID = ObjRecord.UserId;

                }
                else
                {
                    ObjDetails.SupplierID = ObjRecord.UserId;
                    ObjDetails.CombinationID = CombinationId;
                    ObjDetails.CustomerID = ObjRecord.Customer_SupplierId;
                }
            }
            return ObjDetails;
        }


       public  Details GetRequiredIds(string UserId, string CombinationId)
        {
           return GetSupplierIdFromCombinationId(CombinationId);
        }
    }
}
