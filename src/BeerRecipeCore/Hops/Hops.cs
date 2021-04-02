namespace BeerRecipeCore.Hops
{
    public record Hops(string Name, HopsCharacteristics Characteristics, string Notes, string Origin)
        : IngredientTypeBase(Name, Notes);
}
