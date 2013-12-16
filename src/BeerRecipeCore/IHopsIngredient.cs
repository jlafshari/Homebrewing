namespace BeerRecipeCore
{
    public interface IHopsIngredient
    {
        /// <summary>
        /// The amount of hops used, in ounces.
        /// </summary>
        float Amount { get; set; }

        /// <summary>
        /// The amount of time the hops is used, in minutes.
        /// </summary>
        int Time { get; set; }

        HopsFlavorType FlavorType { get; set; }
        HopsForm Form { get; set; }
        Hops HopsInfo { get; }
    }
}
