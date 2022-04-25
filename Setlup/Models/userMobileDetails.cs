using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Setlup.Models
{
    public class userMobileDetails
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string userId { get; set; } = String.Empty;

        [BsonElement("mobileNumber")]
        public string mobileNumber { get; set; } = String.Empty;

        [BsonElement("status")]
        public int status { get; set; }

        [BsonElement("CreatedDate")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedDate { get; set; }
    }
}
