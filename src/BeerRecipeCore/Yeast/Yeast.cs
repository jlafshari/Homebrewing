namespace BeerRecipeCore.Yeast
{
    public record Yeast(string Name, YeastCharacteristics Characteristics, string Notes, string Laboratory, string ProductId = null)
        : IngredientTypeBase(Name, Notes);
}
