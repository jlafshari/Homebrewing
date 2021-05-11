using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomebrewApi.Models
{
    public class Hops
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; }
        public string Name { get; init; }
        public HopsCharacteristics Characteristics { get; init; }
        public string Notes { get; init; }
        public string Origin { get; init; }
    }

    public record HopsCharacteristics(float AlphaAcid, float BetaAcid, float Hsi);
}