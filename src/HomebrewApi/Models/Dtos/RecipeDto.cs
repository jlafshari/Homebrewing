namespace HomebrewApi.Models.Dtos
{
    public record RecipeDto(string Id, float Size, string Name, RecipeProjectedOutcome ProjectedOutcome)
    {
        public string StyleName { get; set; }
    }
}