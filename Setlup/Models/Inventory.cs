using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Setlup.Models
{
    public class Inventory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string InventoryId { get; set; } = String.Empty;

        [BsonElement("UserID"), BsonRepresentation(BsonType.ObjectId)]
        public string UserID { get; set; } = String.Empty;

        [BsonElement("Items")]
        public string ItemName { get; set; } = String.Empty;

        [BsonElement("Quantity")]
        public int Quantity { get; set; }

        [BsonElement("Price")]
        public double Price { get; set; }

        [BsonElement("Size")]
        public string Size { get; set; } = String.Empty;

        [BsonElement("Status")]
        public int Status { get; set; }

        [BsonElement("CreatedDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedDate { get; set; }






    }
    public class InventoryList
    {

        public List<Inventory> InvenotryItems { get; set; }
    }
}
