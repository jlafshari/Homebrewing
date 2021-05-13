using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomebrewApi.Models
{
    public record CommonHop(int BoilAdditionTime, int IbuContributionPercentage)
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string HopId { get; init; }
    }
}