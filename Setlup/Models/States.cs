using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Setlup.Models
{
    public class States_list
    {
        public List<States> StatesList { get; set; }
    }

    public class States
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string StateID { get; set; } = String.Empty;
        [BsonElement("StateName")]
        public string StateName { get; set; } = String.Empty;
    }
}
