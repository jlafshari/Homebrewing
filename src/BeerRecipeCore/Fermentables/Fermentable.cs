namespace BeerRecipeCore.Fermentables
{
    public record Fermentable(string Name, FermentableCharacteristics Characteristics, string Notes, string Origin)
        : IngredientTypeBase(Name, Notes);
}
