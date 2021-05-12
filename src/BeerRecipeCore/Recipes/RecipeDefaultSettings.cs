namespace BeerRecipeCore.Recipes
{
    /// <summary>
    /// Default recipe settings based on a 5 gallon batch.
    /// </summary>
    public static class RecipeDefaultSettings
    {
        /// <summary>
        /// The size in gallons.
        /// </summary>
        public const float Size = 5f;

        /// <summary>
        /// The boil time in minutes.
        /// </summary>
        public const int BoilTime = 60;

        /// <summary>
        /// The mash extraction efficiency percentage.
        /// </summary>
        public const float ExtractionEfficiency = 60f;

        /// <summary>
        /// The amount of yeast used, in pounds.
        /// </summary>
        public const float YeastWeight = 0.1f;

        /// <summary>
        /// The amount of hop used, in ounces.
        /// </summary>
        public const float HopAmount = 1.0f;
    }
}
