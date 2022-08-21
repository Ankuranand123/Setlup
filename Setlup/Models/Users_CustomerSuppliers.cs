using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Setlup.Models
{
    public class Users_CustomerSuppliers
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerSuppliersId { get; set; } = String.Empty;  // combinationId

        [BsonElement("UserId"), BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = String.Empty;
        [BsonElement("AddedUserPhNo")]
        public string AddedUserPh { get; set; } = String.Empty;
        [BsonElement("Customer_SupplierId"), BsonRepresentation(BsonType.ObjectId)]
        public string Customer_SupplierId { get; set; } = String.Empty;   //Added Customer or SupplierId

        //[BsonElement("BusinessTypeId"), BsonRepresentation(BsonType.ObjectId)]
        //public string BusinessTypeId { get; set; } = String.Empty;

        [BsonElement("Customer_or_Supplier")]
        public int Customer_or_Supplier { get; set; }

        [BsonElement("AddedUserName")]
        public string AddedUserName { get; set; } = String.Empty;

        [BsonElement("UserName")]
        public string UserName { get; set; } = String.Empty;

        [BsonElement("Status")]
        public int Status { get; set; }
    }
}
