using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Setlup.Models
{
    public class Orders
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderId { get; set; } = String.Empty;

        [BsonElement("combinationId"), BsonRepresentation(BsonType.ObjectId)]
        public string CombinationId { get; set; } = String.Empty;

        [BsonElement("Items")]
        public Items[] Items { get; set; } 


        [BsonElement("OrderStatus")]
        public int OrderStatus { get; set; }  // 1 Accepted 2 Rejected 3 Delivered

        [BsonElement("InvoiceStatus")]
        public int InvoiceStatus { get; set; }  // 0 Not Generated 1 Generated

        [BsonElement("RevisedOrder")]
        public RevisedOrder[] RevisedOrder { get; set; }


        [BsonElement("totalOrderPrice")]
        public int TotalOrderPrice { get; set; }
        [BsonElement("Invoice")]
        public Invoice[] InvoiceDetails { get; set; }

        [BsonElement("CreatedBy"), BsonRepresentation(BsonType.ObjectId)]
        public string CreatedBy { get; set; } = String.Empty;

        [BsonElement("CreatedDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedDate { get; set; }
    }

    public class Items
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string ItemId { get; set; } = String.Empty;

        [BsonElement("ItemName")]
        public string ItemName { get; set; } = String.Empty;

        [BsonElement("ItemSize")]
        public string ItemSize { get; set; } = String.Empty;
        [BsonElement("ItemQty")]
        public int ItemQty { get; set; }
        [BsonElement("ItemPrice")]
        public int ItemPrice { get; set; }
        [BsonElement("ItemTotalPrice")]
        public int ItemTotalPrice { get; set; }
    }

    public class Invoice
    {
        [BsonElement("ItemName")]
        public string ItemName { get; set; } = String.Empty;

        [BsonElement("ItemSize")]
        public string ItemSize { get; set; } = String.Empty;
        [BsonElement("ItemQty")]
        public int ItemQty { get; set; }
        [BsonElement("ItemPrice")]
        public int ItemPrice { get; set; }
        [BsonElement("ItemTotalPrice")]
        public int ItemTotalPrice { get; set; }

    }
    public class RevisedOrder
    {

        [BsonElement("ItemName")]
        public string ItemName { get; set; } = String.Empty;

        [BsonElement("ItemSize")]
        public string ItemSize { get; set; } = String.Empty;
        [BsonElement("ItemQty")]
        public int ItemQty { get; set; }
        [BsonElement("ItemPrice")]
        public int ItemPrice { get; set; }
        [BsonElement("ItemTotalPrice")]
        public int ItemTotalPrice { get; set; }


    }
}
