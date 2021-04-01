namespace BeerRecipeCore.Hops
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

        /// <summary>
        /// The dry hop time in days, if the hops use is "dry hop", otherwise it's null.
        /// </summary>
        int? DryHopTime { get; set; }

        HopsFlavorType FlavorType { get; set; }
        HopsForm Form { get; set; }
        HopsUse Use { get; set; }
        Hops HopsInfo { get; }
    }
}
