using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BeerRecipeCore.Services;
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
        private readonly RecipeService _recipeService;

        public HomebrewingDbService(IHomebrewingDatabaseSettings settings, IMapper mapper)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
            _mapper = mapper;
            _recipeService = new RecipeService();
        }

        public void CreateBeerStyle(Style beerStyle)
        {
            var styleCollection = _database.GetCollection<Style>(StyleCollectionName);
            styleCollection.InsertOne(beerStyle);
        }

        public List<StyleDto> GetBeerStyles()
        {
            var styleCollection = _database.GetCollection<Style>(StyleCollectionName);
            return styleCollection.FindSync(s => true).ToEnumerable()
                .Select(s => _mapper.Map<StyleDto>(s)).ToList();
        }

        public RecipeDto GenerateRecipe(RecipeGenerationInfoDto recipeGenerationInfoDto)
        {
            var style = GetStyle(recipeGenerationInfoDto.StyleId);
            var generatedRecipe = _recipeService.GenerateRecipe(recipeGenerationInfoDto.Size, style, recipeGenerationInfoDto.Abv,
                recipeGenerationInfoDto.ColorSrm, recipeGenerationInfoDto.Name);
            var recipeToInsert = _mapper.Map<Recipe>(generatedRecipe);
            InsertRecipe(recipeToInsert);
            var recipeDto = _mapper.Map<RecipeDto>(recipeToInsert);
            recipeDto.StyleName = style.Name;
            return recipeDto;
        }

        private void InsertRecipe(Recipe recipeToGenerate)
        {
            var recipeCollection = _database.GetCollection<Recipe>(RecipeCollectionName);
            recipeCollection.InsertOne(recipeToGenerate);
        }

        private Style GetStyle(string styleId)
        {
            var styleCollection = _database.GetCollection<Style>(StyleCollectionName);
            return styleCollection.FindSync(s => s.Id == styleId).SingleOrDefault();
        }

        public List<RecipeDto> GetRecipes()
        {
            var filter = Builders<Recipe>.Filter.Where(_ => true);
            return GetRecipes(filter);
        }

        public RecipeDto GetRecipe(string recipeId)
        {
            var filter = Builders<Recipe>.Filter.Eq(r => r.Id, recipeId);
            return GetRecipes(filter).SingleOrDefault();
        }

        public void DeleteRecipe(string recipeId)
        {
            var recipeCollection = _database.GetCollection<Recipe>(RecipeCollectionName);
            recipeCollection.DeleteOne(r => r.Id == recipeId);
        }
        
        private List<RecipeDto> GetRecipes(FilterDefinition<Recipe> filter)
        {
            var styles = GetBeerStyles();
            var recipeCollection = _database.GetCollection<Recipe>(RecipeCollectionName);
            var recipes = recipeCollection.FindSync(filter).ToEnumerable().Select(r => GetRecipeDto(r, styles)).ToList();
            return recipes;
        }

        private RecipeDto GetRecipeDto(Recipe r, IEnumerable<StyleDto> styles)
        {
            var recipe = _mapper.Map<Recipe, RecipeDto>(r);
            recipe.StyleName = styles.FirstOrDefault(s => s.Id == r.StyleId)?.Name;
            return recipe;
        }
    }
}