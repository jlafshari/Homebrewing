using System.Collections.Generic;
using HomebrewApi.Models;
using MongoDB.Driver;

namespace HomebrewApi.Services
{
    public class HomebrewingDbService
    {
        private const string StyleCollectionName = "Styles";
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
    }
}