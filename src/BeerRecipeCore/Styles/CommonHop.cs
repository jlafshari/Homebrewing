using BeerRecipeCore.Hops;

namespace BeerRecipeCore.Styles
{
    public class CommonHop
    {
        public Hop Hop { get; set; }
        public int BoilAdditionTime { get; set; }
        public int IbuContributionPercentage { get; set; }
        public float AlphaAcid => Hop.Characteristics.AlphaAcid;
    }
}