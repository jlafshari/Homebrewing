using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomebrewApi.Models
{
    public class Hop
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; }
        public string Name { get; init; }
        public HopCharacteristics Characteristics { get; init; }
        public string Notes { get; init; }
        public string Origin { get; init; }
    }
}