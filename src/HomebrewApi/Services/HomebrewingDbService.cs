using System.Collections.Generic;
using HomebrewApi.Models;
using MongoDB.Driver;

namespace HomebrewApi.Services
{
    public class HomebrewingDbService
    {
        private const string StyleCollectionName = "Styles";
        private const string RecipeCollectionName = "Recipes";
        private readonly IMongoDatabase _database;

        public HomebrewingDbService(IHomebrewingDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
        }

        public void CreateBeerStyle(Style beerStyle)
        {
            var styleCollection = _database.GetCollection<Style>(StyleCollectionName);
            styleCollection.InsertOne(beerStyle);
        }

        public List<Style> GetBeerStyles()
        {
            var styleCollection = _database.GetCollection<Style>(StyleCollectionName);
            return styleCollection.FindSync(s => true).ToList();
        }

        public Recipe GenerateRecipe(RecipeGenerationInfo recipeGenerationInfo)
        {
            //TODO: create method in BeerRecipeCore to generate recipe based on outcome
            var recipe = new Recipe { GenerationInfo = recipeGenerationInfo };
            var recipeCollection = _database.GetCollection<Recipe>(RecipeCollectionName);
            recipeCollection.InsertOne(recipe);
            return recipe;
        }

        public List<Recipe> GetRecipes()
        {
            var recipeCollection = _database.GetCollection<Recipe>(RecipeCollectionName);
            return recipeCollection.FindSync(_ => true).ToList();
        }
    }
}