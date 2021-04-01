namespace BeerRecipeCore.Fermentables
{
    public class FermentableCharacteristics
    {
        public FermentableCharacteristics(float? yield, float color, float? diastaticPower)
        {
            Yield = yield;
            Color = color;
            DiastaticPower = diastaticPower;
            YieldByWeight = null;
        }

        /// <summary>
        /// The percent dry yield if the fermentable is a grain.
        /// </summary>
        public float? Yield { get; }

        /// <summary>
        /// The percent raw yield by weight if this is an extract, adjunct, or sugar.
        /// </summary>
        public float? YieldByWeight { get; set; }

        public double Color { get; }

        /// <summary>
        /// The diastatic power in Lintner degree units. If the FermentableType is not a grain or adjunct, then this value is null.
        /// </summary>
        public float? DiastaticPower { get; }

        public FermentableType Type { get; set; }

        public MaltCategory? MaltCategory { get; set; }

        public int GravityPoint { get; set; }
    }
}
