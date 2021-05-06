using BeerRecipeCore.Yeast;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomebrewApi.Models
{
    public class Yeast
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; }
        public string Name { get; init; }
        public YeastCharacteristics Characteristics { get; init; }
        public string Notes { get; init; }
        public string Laboratory { get; init; }
    }
}