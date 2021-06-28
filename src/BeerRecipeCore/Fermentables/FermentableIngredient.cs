namespace BeerRecipeCore.Fermentables
{
    public class FermentableIngredient : IFermentableIngredient
    {
        public float Amount { get; set; }
        public Fermentable FermentableInfo { get; set; }
    }
}