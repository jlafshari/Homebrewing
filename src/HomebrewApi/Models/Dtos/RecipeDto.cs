namespace HomebrewApi.Models.Dtos
{
    public class RecipeDto
    {
        public string Id { get; init; }
        public float Size { get; init; }
        public string Name { get; init; }
        public string StyleName { get; set; }
        public RecipeProjectedOutcome ProjectedOutcome { get; init; }
    }
}