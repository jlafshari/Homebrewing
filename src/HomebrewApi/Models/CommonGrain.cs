using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomebrewApi.Models
{
    public record CommonGrain(int ProportionOfGrist)
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string FermentableId { get; init; }
    }
}