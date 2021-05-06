namespace BeerRecipeCore.Yeast
{
    public interface IYeastIngredient
    {
        /// <summary>
        /// The amount of yeast used, in pounds.
        /// </summary>
        float Weight { get; init; }

        /// <summary>
        /// The amount of yeast used, in gallons.
        /// </summary>
        float Volume { get; init; }

        Yeast YeastInfo { get; init; }
    }
}
