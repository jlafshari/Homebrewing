using BeerRecipeCore.Fermentables;

namespace HomebrewApi.Models
{
    public class CommonGrain
    {
        public Fermentable Fermentable { get; set; }
        public int ProportionOfGrist { get; set; }

        public MaltCategory Category => (MaltCategory) Fermentable.Characteristics.MaltCategory;
    }
}