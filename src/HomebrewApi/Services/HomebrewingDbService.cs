using System.Collections.Generic;
using AutoMapper;
using HomebrewApi.Models;
using HomebrewApi.Models.Dtos;
using MongoDB.Driver;

namespace HomebrewApi.Services
{
    public class HomebrewingDbService
    {
        private readonly IMapper _mapper;
        private const string StyleCollectionName = "Styles";
        private const string RecipeCollectionName = "Recipes";
        private readonly IMongoDatabase _database;

        public HomebrewingDbService(IHomebrewingDatabaseSettings settings, IMapper mapper)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
            _mapper = mapper;
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

        public RecipeDto GenerateRecipe(RecipeGenerationInfoDto recipeGenerationInfoDto)
        {
            //TODO: create method in BeerRecipeCore to generate recipe based on outcome
            var recipeToGenerate = _mapper.Map<Recipe>(recipeGenerationInfoDto);
            var recipeCollection = _database.GetCollection<Recipe>(RecipeCollectionName);
            recipeCollection.InsertOne(recipeToGenerate);
            var recipe = _mapper.Map<RecipeDto>(recipeToGenerate);
            var style = GetStyle(recipeGenerationInfoDto.StyleId);
            recipe.StyleName = style?.Name;
            return recipe;
        }

        private Style GetStyle(string styleId)
        {
            var styleCollection = _database.GetCollection<Style>(StyleCollectionName);
            return styleCollection.FindSync(s => s.Id == styleId).SingleOrDefault();
        }

        public List<Recipe> GetRecipes()
        {
            var recipeCollection = _database.GetCollection<Recipe>(RecipeCollectionName);
            return recipeCollection.FindSync(_ => true).ToList();
        }
    }
}