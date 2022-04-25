using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Setlup.Models
{
    public class userDetails
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string detailsId { get; set; } = String.Empty;

        [BsonElement("userId"), BsonRepresentation(BsonType.ObjectId)]
          public string userId { get; set; } = String.Empty;

        [BsonElement("businessId"), BsonRepresentation(BsonType.ObjectId)]
        public string businessId { get; set; } = String.Empty;
        [BsonElement("name")]
        public string name { get; set; } = String.Empty;
        [BsonElement("email")]
        public string email { get; set; } = String.Empty;
        [BsonElement("designation")]
        public string designation { get; set; } = String.Empty;
        [BsonElement("panNumber")]
        public string panNumber { get; set; } = String.Empty;

        [BsonElement("brandDeals")]
        public string[]? brandDeals { get; set; }
        [BsonElement("address")]
        public string address { get; set; } = String.Empty;

        [BsonElement("CreatedDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedDate { get; set; }
    }
}
