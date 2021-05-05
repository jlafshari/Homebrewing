namespace BeerRecipeCore.Fermentables
{
    internal class FermentableIngredient : IFermentableIngredient
    {
        public float Amount { get; set; }
        public Fermentable FermentableInfo { get; set; }
    }
}