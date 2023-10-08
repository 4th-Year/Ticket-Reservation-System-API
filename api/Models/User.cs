using api.Data;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Models
{
    [BsonIgnoreExtraElements]
    [BsonCollection("users")]
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonElement("_id")]
        public string Id { get; set; } = string.Empty;

        [BsonElement("firstname")]
        public string FirstName { get; set; } = string.Empty;

        [BsonElement("lastname")]
        public string LastName { get; set; } = string.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("address")]
        public string Address { get; set; } = string.Empty;

        [BsonElement("nic")]
        public string NIC { get; set; } = string.Empty;

        [BsonElement("dob")]
        public DateTime DOB { get; set; }

        [BsonElement("gender")]
        public byte Gender { get; set; } = 1;

        [BsonElement("mobile")]
        public string Mobile { get; set; } = string.Empty;

        [BsonElement("password")]
        public string Password { get; set; } = string.Empty;

        [BsonElement("isAdmin")]

        public bool IsAdmin { get; set; } = false;

    }
}
