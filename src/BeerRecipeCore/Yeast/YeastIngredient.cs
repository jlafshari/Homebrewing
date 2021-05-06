namespace BeerRecipeCore.Yeast
{
    internal record YeastIngredient(float Weight, float Volume, Yeast YeastInfo) : IYeastIngredient;
}