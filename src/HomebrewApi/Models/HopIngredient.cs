using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomebrewApi.Models
{
    internal class HopIngredient
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string HopId { get; set; }
        public float Amount { get; set; }
        public int BoilAdditionTime { get; set; }
    }
}