using BeerRecipeCore.Hops;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomebrewApi.Models
{
    public record CommonHop(HopFlavorType HopFlavorType)
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string HopId { get; init; }
    }
}