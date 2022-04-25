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

        public  void InsertTextMessage(string UserId, MessageText ObjMessageText)
        {
            //If message is a simple text message then message  type is 1
            var uid = cryptingData.Decrypt(UserId);
            //If user has clicked on Customer then Customer_Supplier type is 1 but since its supplier who is typing message so Customer_Supplier typ should be 2
           if(ObjMessageText.Customer_or_Supplier == 1)
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

        }
    }
}
