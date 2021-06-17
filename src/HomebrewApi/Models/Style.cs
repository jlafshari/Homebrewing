using System.Collections.Generic;
using BeerRecipeCore.Styles;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomebrewApi.Models
{
    public record Style
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; }
        public string Name { get; init; }
        public StyleCategory Category { get; init; }
        public StyleClassification Classification { get; init; }
        public List<StyleThreshold> Thresholds { get; init; }
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public List<CommonGrain> CommonGrains { get; init; } = new();
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public List<CommonHop> CommonHops { get; init; } = new();
        [BsonRepresentation(BsonType.ObjectId)]
        public string CommonYeastId { get; init; }
    }
}