using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomebrewApi.Models
{
    internal record FermentableIngredient(float Amount)
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string FermentableId { get; set; }
    }
}