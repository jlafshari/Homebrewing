namespace HomebrewApi.Models
{
    public class HomebrewingDatabaseSettings : IHomebrewingDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IHomebrewingDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}