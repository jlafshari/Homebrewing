using BeerRecipeCore.Fermentables;

namespace HomebrewApi.Models
{
    public class FermentableCharacteristics
    {
        /// <summary>
        /// The percent dry yield if the fermentable is a grain.
        /// </summary>
        public float? Yield { get; init; }

        /// <summary>
        /// The percent raw yield by weight if this is an extract, adjunct, or sugar.
        /// </summary>
        public float? YieldByWeight { get; init; }

        public double Color { get; init; }

        /// <summary>
        /// The diastatic power in Lintner degree units. If the FermentableType is not a grain or adjunct, then this value is null.
        /// </summary>
        public float? DiastaticPower { get; init; }

        public FermentableType Type { get; init; }

        public MaltCategory? MaltCategory { get; set; }

        public int GravityPoint { get; init; }
    }
}