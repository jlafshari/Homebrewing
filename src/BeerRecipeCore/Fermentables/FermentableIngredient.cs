namespace BeerRecipeCore.Fermentables
{
    public class FermentableIngredient : IFermentableIngredient
    {
        public FermentableIngredient(Fermentable fermentableInfo)
        {
            FermentableInfo = fermentableInfo;
        }

        /// <summary>
        /// The amount of fermentable used, in pounds.
        /// </summary>
        public float Amount { get; set; }

        public Fermentable FermentableInfo { get; }
    }
}
