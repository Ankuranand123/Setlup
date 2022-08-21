using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Setlup.Models
{
    [BsonIgnoreExtraElements]
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
        [BsonElement("CreatedBy"), BsonRepresentation(BsonType.ObjectId)]
        public string CreatedBy { get; set; } = String.Empty;
        //[BsonElement("CreatedUser"), BsonRepresentation(BsonType.ObjectId)]
        //public string CreatedUser { get; set; } = String.Empty;


        [BsonElement("CreatedDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedDate { get; set; }

        [BsonElement("Status")]
        public int Status { get; set; }

        [BsonIgnore]
        public Items[]? ItemList { get; set; } 

        [BsonIgnore]
        public int OrderStatus { get; set; }

        [BsonIgnore]
        public string InvoiceNo { get; set; } = String.Empty;

        [BsonIgnore]
        public int ItemsCount { get; set; }

        [BsonIgnore]
        public int IsTax { get; set; }

        [BsonIgnore]
        public int IsDues { get; set; }

        [BsonIgnore]
        public int Amount { get; set; }

        [BsonElement("InvoiceDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime InvoiceDate { get; set; }

        [BsonIgnore]
        public int IsDue { get; set; }


        //[BsonElement("SupplierId")]
        //public string SupplierId { get; set; } = String.Empty;

        // 1 :Simple message, 2: order placed, 3: order updated, 4 : order accepted(invoice)

    }
    public class MessageTextList
    {
        //public string SupplierId { get; set; }

        //public string CustomerId { get; set; }
        //public string CombinationId { get; set; }
        public List<MessageText> ObjmsgtextList { get; set; }
    }

    //public class ItemList
    //{

    //}
}
