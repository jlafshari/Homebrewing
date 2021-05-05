using BeerRecipeCore.Fermentables;

namespace BeerRecipeCore.Styles
{
    public class CommonGrain
    {
        public Fermentable Fermentable { get; set; }
        public float ProportionOfGrist { get; set; }

        public MaltCategory Category => (MaltCategory) Fermentable.Characteristics.MaltCategory;
        public int GravityPoint => Fermentable.Characteristics.GravityPoint;
    }
}