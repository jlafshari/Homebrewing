using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BeerRecipeCore.Recipes;
using BeerRecipeCore.Services;
using HomebrewApi.Models;
using HomebrewApi.Models.Dtos;
using MongoDB.Driver;
using CommonGrain = HomebrewApi.Models.CommonGrain;
using Style = HomebrewApi.Models.Style;

namespace HomebrewApi.Services
{
    public class HomebrewingDbService
    {
        private readonly IMapper _mapper;
        private const string StyleCollectionName = "Styles";
        private const string FermentableCollectionName = "Fermentables";
        private const string YeastCollectionName = "Yeasts";
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
            var recipeToInsert = GenerateRecipe(recipeGenerationInfoDto, style);
            InsertRecipe(recipeToInsert);
            var recipeDto = GetRecipeDto(recipeToInsert, new List<StyleDto> { _mapper.Map<StyleDto>(style) });
            return recipeDto;
        }

        public void CreateYeast(Yeast yeast)
        {
            var yeastCollection = _database.GetCollection<Yeast>(YeastCollectionName);
            yeastCollection.InsertOne(yeast);
        }

        private Recipe GenerateRecipe(RecipeGenerationInfoDto recipeGenerationInfoDto, Style style)
        {
            var recipeGenerationInfo = GetRecipeGenerationInfo(recipeGenerationInfoDto, style);

            var generatedRecipe = _recipeService.GenerateRecipe(recipeGenerationInfo);
            var recipeToInsert = ConvertRecipeToDbRecipe(recipeGenerationInfoDto, style, generatedRecipe);
            return recipeToInsert;
        }

        private RecipeGenerationInfo GetRecipeGenerationInfo(RecipeGenerationInfoDto recipeGenerationInfoDto, Style style)
        {
            var styleForBeerRecipeCore = _mapper.Map<Style, BeerRecipeCore.Styles.Style>(style);
            LoadFermentables(styleForBeerRecipeCore, style.CommonGrains);
            styleForBeerRecipeCore.CommonYeast = GetYeast(style.CommonYeastId);
            var recipeGenerationInfo = _mapper.Map<RecipeGenerationInfo>(recipeGenerationInfoDto);
            recipeGenerationInfo.Style = styleForBeerRecipeCore;
            return recipeGenerationInfo;
        }

        private Recipe ConvertRecipeToDbRecipe(RecipeGenerationInfoDto recipeGenerationInfoDto, Style style, IRecipe generatedRecipe)
        {
            var recipeToInsert = _mapper.Map<Recipe>(generatedRecipe);
            recipeToInsert.ProjectedOutcome = _mapper.Map<RecipeProjectedOutcome>(recipeGenerationInfoDto);
            recipeToInsert.StyleId = recipeGenerationInfoDto.StyleId;
            recipeToInsert.YeastId = style.CommonYeastId;
            for (int i = 0; i < recipeToInsert.FermentableIngredients.Count; i++)
            {
                recipeToInsert.FermentableIngredients[i].FermentableId = style.CommonGrains[i].FermentableId;
            }

            return recipeToInsert;
        }

        private void InsertRecipe(Recipe recipeToGenerate)
        {
            var recipeCollection = _database.GetCollection<Recipe>(RecipeCollectionName);
            recipeCollection.InsertOne(recipeToGenerate);
        }

        private Style GetStyle(string styleId)
        {
            var styleCollection = _database.GetCollection<Style>(StyleCollectionName);
            var style = styleCollection.FindSync(s => s.Id == styleId).SingleOrDefault();
            return style;
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

        public void CreateFermentable(Fermentable fermentable)
        {
            var fermentableCollection = _database.GetCollection<Fermentable>(FermentableCollectionName);
            fermentableCollection.InsertOne(fermentable);
        }

        private void LoadFermentables(BeerRecipeCore.Styles.Style style, List<CommonGrain> commonGrains)
        {
            foreach (var commonGrain in commonGrains)
            {
                var fermentable = GetFermentable(commonGrain.FermentableId);
                style.CommonGrains.Add(new BeerRecipeCore.Styles.CommonGrain
                {
                    Fermentable = fermentable, ProportionOfGrist = commonGrain.ProportionOfGrist
                });
            }
        }

        private BeerRecipeCore.Fermentables.Fermentable GetFermentable(string fermentableId)
        {
            var fermentableCollection = _database.GetCollection<Fermentable>(FermentableCollectionName);
            var filter = Builders<Fermentable>.Filter.Eq(r => r.Id, fermentableId);
            var fermentableFromDb = fermentableCollection.FindSync(filter).ToEnumerable().SingleOrDefault();
            return _mapper.Map<Fermentable, BeerRecipeCore.Fermentables.Fermentable>(fermentableFromDb);
        }

        private BeerRecipeCore.Yeast.Yeast GetYeast(string yeastId)
        {
            var yeastCollection = _database.GetCollection<Yeast>(YeastCollectionName);
            var filter = Builders<Yeast>.Filter.Eq(y => y.Id, yeastId);
            var yeastFromDb = yeastCollection.FindSync(filter).ToEnumerable().SingleOrDefault();
            return _mapper.Map<Yeast, BeerRecipeCore.Yeast.Yeast>(yeastFromDb);
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
            var yeast = GetYeast(r.YeastId);
            recipe.YeastIngredient = _mapper.Map<YeastIngredientDto>(yeast);
            foreach (var fermentableIngredient in r.FermentableIngredients)
            {
                var fermentable = GetFermentable(fermentableIngredient.FermentableId);
                recipe.FermentableIngredients.Add(new FermentableIngredientDto(fermentableIngredient.Amount, fermentable.Name));
            }
            return recipe;
        }
    }
}