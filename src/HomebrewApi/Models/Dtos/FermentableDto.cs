namespace HomebrewApi.Models.Dtos
{
    public record FermentableDto(string Id, string Name, string Notes)
    {
        public double Color { get; init; }
        public string MaltCategory { get; init; }
    }
}