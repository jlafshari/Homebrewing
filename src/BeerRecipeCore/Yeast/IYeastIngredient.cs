namespace BeerRecipeCore.Yeast
{
    public interface IYeastIngredient
    {
        /// <summary>
        /// The amount of yeast used, in pounds.
        /// </summary>
        float Weight { get; set; }

        /// <summary>
        /// The amount of yeast used, in gallons.
        /// </summary>
        float Volume { get; set; }

        Yeast YeastInfo { get; set; }
    }
}
