using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Setlup.Models
{
    public class MessageText
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MessageId { get; set; } = String.Empty;

        [BsonElement("combinationId"), BsonRepresentation(BsonType.ObjectId)]
        public string CombinationId { get; set; } = String.Empty;

        [BsonElement("Message")]
        public string Message { get; set; } = String.Empty;
        [BsonElement("Customer_or_Supplier")]
        public int Customer_or_Supplier { get; set; }
        [BsonElement("MessageType")]
        public int MessageType { get; set; }

        [BsonElement("OrderId"), BsonRepresentation(BsonType.ObjectId)]
        public string OrderId { get; set; } = String.Empty;
        [BsonElement("CreatedBy")]
        public string CreatedBy { get; set; } = String.Empty;
        [BsonElement("CreatedUser"), BsonRepresentation(BsonType.ObjectId)]
        public string CreatedUser { get; set; } = String.Empty;


        [BsonElement("CreatedDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedDate { get; set; }

        [BsonElement("Status")]
        public int Status { get; set; }


        //[BsonElement("SupplierId")]
        //public string SupplierId { get; set; } = String.Empty;



    }
    public class MessageTextList
    {
        public string SupplierId { get; set; }
        public List<MessageText> ObjmsgtextList { get; set; }
    }
}
