namespace BeerRecipeCore
{
    /// <summary>
    /// Default recipe settings based on a 5 gallon batch.
    /// </summary>
    public static class RecipeDefaultSettings
    {
        /// <summary>
        /// The size in gallons.
        /// </summary>
        public static float Size
        {
            get { return 5f; }
        }

        /// <summary>
        /// The boil time in minutes.
        /// </summary>
        public static int BoilTime
        {
            get { return 60; }
        }

        /// <summary>
        /// The mash extraction efficiency percentage.
        /// </summary>
        public static float ExtractionEfficiency
        {
            get { return 60f; }
        }

        /// <summary>
        /// The amount of yeast used, in pounds.
        /// </summary>
        public static float YeastWeight
        {
            get { return 0.1f; }
        }
    }
}
