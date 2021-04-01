namespace BeerRecipeCore.Hops
{
    public class HopsIngredient : IHopsIngredient
    {
        public HopsIngredient(Hops hopsInfo)
        {
            HopsInfo = hopsInfo;
        }

        /// <summary>
        /// The amount of hops used, in ounces.
        /// </summary>
        public float Amount { get; set; }

        /// <summary>
        /// The amount of time the hops is used, in minutes.
        /// </summary>
        public int Time { get; set; }

        public int? DryHopTime { get; set; }

        public HopsFlavorType FlavorType { get; set; }

        public HopsForm Form { get; set; }

        public HopsUse Use { get; set; }

        public Hops HopsInfo { get; }
    }
}
