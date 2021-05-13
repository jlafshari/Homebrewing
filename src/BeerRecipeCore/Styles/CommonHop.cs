using BeerRecipeCore.Hops;

namespace BeerRecipeCore.Styles
{
    public class CommonHop
    {
        public Hop Hop { get; set; }
        public HopFlavorType HopFlavorType { get; set; }
        public int BoilAdditionTime { get; set; }
        public int IbuContributionPercentage { get; set; }
    }
}