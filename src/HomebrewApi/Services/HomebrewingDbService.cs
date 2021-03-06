using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Recipes;
using BeerRecipeCore.Services;
using HomebrewApi.Models;
using HomebrewApi.Models.Dtos;
using MongoDB.Bson;
using MongoDB.Driver;
using CommonGrain = HomebrewApi.Models.CommonGrain;
using Fermentable = HomebrewApi.Models.Fermentable;
using FermentableIngredient = HomebrewApi.Models.FermentableIngredient;
using Style = HomebrewApi.Models.Style;

namespace HomebrewApi.Services
{
    public class HomebrewingDbService
    {
        private readonly IMapper _mapper;
        private const string StyleCollectionName = "Styles";
        private const string FermentableCollectionName = "Fermentables";
        private const string YeastCollectionName = "Yeasts";
        private const string HopCollectionName = "Hops";
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
                .Select(s => _mapper.Map<StyleDto>(s))
                .OrderBy(s => s.Name)
                .ToList();
        }

        public RecipeDto GenerateRecipe(RecipeGenerationInfoDto recipeGenerationInfoDto, string userId)
        {
            var style = GetStyle(recipeGenerationInfoDto.StyleId);
            var recipeToInsert = GenerateRecipe(recipeGenerationInfoDto, style);
            recipeToInsert.UserId = userId;
            InsertRecipe(recipeToInsert);
            var recipeDto = GetRecipeDto(recipeToInsert, style.Name);
            return recipeDto;
        }

        public bool UpdateRecipe(string recipeId, RecipeUpdateInfoDto recipeUpdateInfoDto, string userId)
        {
            var recipeProjectedOutcome = GetRecipeProjectedOutcome(recipeId, recipeUpdateInfoDto, userId);

            var recipeCollection = _database.GetCollection<Recipe>(RecipeCollectionName);
            var filter = Builders<Recipe>.Filter.Eq(r => r.Id, recipeId) & Builders<Recipe>.Filter.Eq(r => r.UserId, userId);
            var update = Builders<Recipe>.Update.Set(r => r.Name, recipeUpdateInfoDto.Name)
                .Set(r => r.ProjectedOutcome, recipeProjectedOutcome)
                .PullFilter(r => r.FermentableIngredients, r => true)
                .PullFilter(r => r.HopIngredients, r => true);
            var updateResult = recipeCollection.UpdateOne(filter, update);
            
            var fermentableIngredients = recipeUpdateInfoDto.FermentableIngredients
                .Select(f => _mapper.Map<FermentableIngredient>(f)).ToList();
            var hopIngredients = recipeUpdateInfoDto.HopIngredients.Select(h => _mapper.Map<HopIngredient>(h))
                .OrderByDescending(h => h.BoilAdditionTime).ToList();
            var updateIngredients = Builders<Recipe>.Update
                .PushEach(r => r.FermentableIngredients, fermentableIngredients)
                .PushEach(r => r.HopIngredients, hopIngredients);
            recipeCollection.UpdateOne(filter, updateIngredients);
            
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        private RecipeProjectedOutcome GetRecipeProjectedOutcome(string recipeId, RecipeUpdateInfoDto recipeUpdateInfoDto, string userId)
        {
            var originalRecipe = GetRecipe(recipeId, userId);
            var brcFermentableIngredients = GetFermentableIngredients(recipeUpdateInfoDto.FermentableIngredients).ToList();
            var yeast = GetYeast(originalRecipe.YeastId);
            var recipeProjectedOutcome = _recipeService.GetRecipeProjectedOutcome(originalRecipe.Size,
                brcFermentableIngredients, yeast, originalRecipe.ProjectedOutcome.Ibu);
            return recipeProjectedOutcome;
        }

        public void CreateYeast(Yeast yeast)
        {
            var yeastCollection = _database.GetCollection<Yeast>(YeastCollectionName);
            yeastCollection.InsertOne(yeast);
        }

        public void CreateHop(Hop hop)
        {
            var hopCollection = _database.GetCollection<Hop>(HopCollectionName);
            hopCollection.InsertOne(hop);
        }

        public List<RecipeDto> GetRecipes(string userId)
        {
            var filter = Builders<Recipe>.Filter.Eq(r => r.UserId, userId);
            return GetRecipeDtos(filter);
        }

        public RecipeDto GetRecipeDto(string recipeId, string userId)
        {
            if (!IsIdValid(recipeId))
                throw new ArgumentException("Invalid parameter", nameof(recipeId));

            var filter = Builders<Recipe>.Filter.Eq(r => r.Id, recipeId) & Builders<Recipe>.Filter.Eq(r => r.UserId, userId);
            return GetRecipeDtos(filter).SingleOrDefault();
        }

        public void DeleteRecipe(string recipeId, string userId)
        {
            if (!IsIdValid(recipeId))
                throw new ArgumentException("Invalid parameter", nameof(recipeId));

            var recipeCollection = _database.GetCollection<Recipe>(RecipeCollectionName);
            var filter = Builders<Recipe>.Filter.Eq(r => r.Id, recipeId) & Builders<Recipe>.Filter.Eq(r => r.UserId, userId);
            recipeCollection.DeleteOne(filter);
        }

        public void CreateFermentable(Fermentable fermentable)
        {
            var fermentableCollection = _database.GetCollection<Fermentable>(FermentableCollectionName);
            fermentableCollection.InsertOne(fermentable);
        }

        public List<FermentableDto> GetFermentables()
        {
            var fermentableCollection = _database.GetCollection<Fermentable>(FermentableCollectionName);
            return fermentableCollection.FindSync(s => true).ToEnumerable()
                .Select(f => _mapper.Map<FermentableDto>(f))
                .OrderBy(f => f.Name)
                .ToList();
        }

        private IEnumerable<IFermentableIngredient> GetFermentableIngredients(List<FermentableIngredientDto> fermentableIngredients)
        {
            foreach (var (amount, _, fermentableId) in fermentableIngredients)
            {
                var fermentable = GetFermentable(fermentableId);
                yield return new BeerRecipeCore.Fermentables.FermentableIngredient
                {
                    Amount = amount, FermentableInfo = fermentable
                };
            }
        }

        public FermentableDto GetFermentableDto(string fermentableId)
        {
            var fermentableCollection = _database.GetCollection<Fermentable>(FermentableCollectionName);
            var filter = Builders<Fermentable>.Filter.Eq(r => r.Id, fermentableId);
            var fermentableFromDb = fermentableCollection.FindSync(filter).ToEnumerable().SingleOrDefault();
            return fermentableFromDb != null ? _mapper.Map<FermentableDto>(fermentableFromDb) : null;
        }

        public List<HopDto> GetHops()
        {
            var hopCollection = _database.GetCollection<Hop>(HopCollectionName);
            return hopCollection.FindSync(s => true).ToEnumerable()
                .Select(f => _mapper.Map<HopDto>(f))
                .OrderBy(h => h.Name)
                .ToList();
        }

        public HopDto GetHopDto(string hopId)
        {
            var hopCollection = _database.GetCollection<Hop>(HopCollectionName);
            var filter = Builders<Hop>.Filter.Eq(r => r.Id, hopId);
            var hopFromDb = hopCollection.FindSync(filter).ToEnumerable().SingleOrDefault();
            return hopFromDb != null ? _mapper.Map<HopDto>(hopFromDb) : null;
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
            var recipeGenerationInfo = _mapper.Map<RecipeGenerationInfo>(recipeGenerationInfoDto);
            recipeGenerationInfo.Style = GetBeerRecipeCoreStyle(style);
            return recipeGenerationInfo;
        }

        private BeerRecipeCore.Styles.Style GetBeerRecipeCoreStyle(Style style)
        {
            var styleForBeerRecipeCore = _mapper.Map<Style, BeerRecipeCore.Styles.Style>(style);
            LoadFermentables(styleForBeerRecipeCore, style.CommonGrains);
            LoadHops(styleForBeerRecipeCore, style.CommonHops);
            styleForBeerRecipeCore.CommonYeast = GetYeast(style.CommonYeastId);
            return styleForBeerRecipeCore;
        }

        private Recipe ConvertRecipeToDbRecipe(RecipeGenerationInfoDto recipeGenerationInfoDto, Style style, IRecipe generatedRecipe)
        {
            var recipeToInsert = _mapper.Map<Recipe>(generatedRecipe);
            recipeToInsert.ProjectedOutcome = _mapper.Map<RecipeProjectedOutcome>(recipeGenerationInfoDto);
            recipeToInsert.StyleId = recipeGenerationInfoDto.StyleId;
            recipeToInsert.YeastId = style.CommonYeastId;
            for (var i = 0; i < recipeToInsert.FermentableIngredients.Count; i++)
            {
                recipeToInsert.FermentableIngredients[i].FermentableId = style.CommonGrains[i].FermentableId;
            }

            for (var i = 0; i < recipeToInsert.HopIngredients.Count; i++)
            {
                recipeToInsert.HopIngredients[i].HopId = style.CommonHops[i].HopId;
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

        private void LoadHops(BeerRecipeCore.Styles.Style style, List<CommonHop> commonHops)
        {
            foreach (var commonHop in commonHops)
            {
                var beerRecipeCoreCommonHop = _mapper.Map<CommonHop, BeerRecipeCore.Styles.CommonHop>(commonHop);
                beerRecipeCoreCommonHop.Hop = GetHop(commonHop.HopId);
                style.CommonHops.Add(beerRecipeCoreCommonHop);
            }
        }

        private BeerRecipeCore.Fermentables.Fermentable GetFermentable(string fermentableId)
        {
            var fermentableCollection = _database.GetCollection<Fermentable>(FermentableCollectionName);
            var filter = Builders<Fermentable>.Filter.Eq(r => r.Id, fermentableId);
            var fermentableFromDb = fermentableCollection.FindSync(filter).ToEnumerable().SingleOrDefault();
            return _mapper.Map<Fermentable, BeerRecipeCore.Fermentables.Fermentable>(fermentableFromDb);
        }

        private BeerRecipeCore.Hops.Hop GetHop(string hopId)
        {
            var hopCollection = _database.GetCollection<Hop>(HopCollectionName);
            var filter = Builders<Hop>.Filter.Eq(r => r.Id, hopId);
            var hopFromDb = hopCollection.FindSync(filter).ToEnumerable().SingleOrDefault();
            return _mapper.Map<Hop, BeerRecipeCore.Hops.Hop>(hopFromDb);
        }

        private BeerRecipeCore.Yeast.Yeast GetYeast(string yeastId)
        {
            var yeastCollection = _database.GetCollection<Yeast>(YeastCollectionName);
            var filter = Builders<Yeast>.Filter.Eq(y => y.Id, yeastId);
            var yeastFromDb = yeastCollection.FindSync(filter).ToEnumerable().SingleOrDefault();
            return _mapper.Map<Yeast, BeerRecipeCore.Yeast.Yeast>(yeastFromDb);
        }
        
        private Recipe GetRecipe(string recipeId, string userId)
        {
            var recipeCollection = _database.GetCollection<Recipe>(RecipeCollectionName);
            var filter = Builders<Recipe>.Filter.Eq(r => r.Id, recipeId) & Builders<Recipe>.Filter.Eq(r => r.UserId, userId);
            var recipe = recipeCollection.FindSync(filter).ToEnumerable().FirstOrDefault();
            return recipe;
        }

        private List<RecipeDto> GetRecipeDtos(FilterDefinition<Recipe> filter)
        {
            var styles = GetBeerStyles();
            var recipeCollection = _database.GetCollection<Recipe>(RecipeCollectionName);
            var recipes = recipeCollection.FindSync(filter).ToEnumerable()
                .Select(r => GetRecipeDto(r, styles.FirstOrDefault(s => s.Id == r.StyleId)?.Name)).ToList();
            return recipes;
        }

        private RecipeDto GetRecipeDto(Recipe r, string styleName)
        {
            var recipe = _mapper.Map<Recipe, RecipeDto>(r);
            recipe.StyleName = styleName;
            var yeast = GetYeast(r.YeastId);
            recipe.YeastIngredient = _mapper.Map<YeastIngredientDto>(yeast);
            recipe.FermentableIngredients.AddRange(GetFermentableIngredientDtos(r.FermentableIngredients));
            recipe.HopIngredients.AddRange(GetHopIngredientDtos(r.HopIngredients));
            return recipe;
        }

        private IEnumerable<FermentableIngredientDto> GetFermentableIngredientDtos(List<FermentableIngredient> fermentableIngredients)
        {
            foreach (var fermentableIngredient in fermentableIngredients)
            {
                var fermentable = GetFermentable(fermentableIngredient.FermentableId);
                yield return new FermentableIngredientDto(fermentableIngredient.Amount, fermentable.Name, fermentableIngredient.FermentableId);
            }
        }

        private IEnumerable<HopIngredientDto> GetHopIngredientDtos(List<HopIngredient> hopIngredients)
        {
            foreach (var hopIngredient in hopIngredients)
            {
                var hop = GetHop(hopIngredient.HopId);
                yield return new HopIngredientDto(hop.Name, hopIngredient.Amount, hopIngredient.BoilAdditionTime, hopIngredient.HopId);
            }
        }

        private static bool IsIdValid(string id)
        {
            return ObjectId.TryParse(id, out _);
        }
    }
}