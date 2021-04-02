namespace BeerRecipeCore.Yeast
{
    public record Yeast(string Name, YeastCharacteristics Characteristics, string Notes, string Laboratory, string ProductId)
        : IngredientTypeBase(Name, Notes);
}
