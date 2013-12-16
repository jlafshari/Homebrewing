namespace BeerRecipeCore
{
    public interface IFermentableIngredient
    {
        /// <summary>
        /// The amount of fermentable used, in pounds.
        /// </summary>
        float Amount { get; set; }

        Fermentable FermentableInfo { get; }
    }
}
