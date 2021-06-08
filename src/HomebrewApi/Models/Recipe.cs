using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HomebrewApi.Models
{
    internal class Recipe
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; }
        public float Size { get; init; }
        public string Name { get; init; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string StyleId { get; set; }
        public RecipeProjectedOutcome ProjectedOutcome { get; set; }
        public List<FermentableIngredient> FermentableIngredients { get; set; }
        public List<HopIngredient> HopIngredients { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string YeastId { get; set; }
        public string UserId { get; set; }
    }
}