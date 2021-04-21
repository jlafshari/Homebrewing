using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomebrewApi.Models
{
    public class RecipeGenerationInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; }
        public float Size { get; init; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string StyleId { get; set; }
        public float Abv { get; init; }
        public int ColorSrm { get; init; }
        public string Name { get; init; }
    }
}