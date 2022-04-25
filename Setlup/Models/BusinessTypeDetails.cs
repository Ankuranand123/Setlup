using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Setlup.Models
{
    public class BusinessTypeDetails
    {
        public List<BusinessDetailsList> BusinessDetailsList { get; set; }
    }

    public class BusinessDetailsList
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BusinessId { get; set; } = String.Empty;
        [BsonElement("businessProfileName")]
        public string BusinessType { get; set; } = String.Empty;
    }
}
