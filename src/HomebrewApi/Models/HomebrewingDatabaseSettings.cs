namespace HomebrewApi.Models
{
    public class HomebrewingDatabaseSettings : IHomebrewingDatabaseSettings
    {
        public string ConnectionString { get; init; }
        public string DatabaseName { get; init; }
    }

    public interface IHomebrewingDatabaseSettings
    {
        public string ConnectionString { get; }
        public string DatabaseName { get; }
    }
}