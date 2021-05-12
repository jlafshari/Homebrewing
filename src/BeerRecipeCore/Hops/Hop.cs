namespace BeerRecipeCore.Hops
{
    public record Hop(string Name, HopCharacteristics Characteristics, string Notes, string Origin)
        : IngredientTypeBase(Name, Notes);
}
