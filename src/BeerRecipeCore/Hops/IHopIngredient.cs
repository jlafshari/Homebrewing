namespace BeerRecipeCore.Hops
{
    public interface IHopIngredient
    {
        /// <summary>
        /// The amount of hop used, in ounces.
        /// </summary>
        float Amount { get; set; }

        /// <summary>
        /// The amount of time the hop is used, in minutes.
        /// </summary>
        int Time { get; set; }

        /// <summary>
        /// The dry hop time in days, if the hop use is "dry hop", otherwise it's null.
        /// </summary>
        int? DryHopTime { get; set; }

        HopFlavorType FlavorType { get; set; }
        HopForm Form { get; set; }
        HopUse Use { get; set; }
        Hop HopInfo { get; }
    }
}
