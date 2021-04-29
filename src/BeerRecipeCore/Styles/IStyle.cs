using System.Collections.Generic;
using BeerRecipeCore.Fermentables;

namespace BeerRecipeCore.Styles
{
    public interface IStyle
    {
        public string Name { get; }
        public StyleCategory Category { get; }
        public StyleClassification Classification { get; }
        public List<StyleThreshold> Thresholds { get; }
        public List<CommonGrain> CommonGrains { get; set; }
    }

    public class CommonGrain
    {
        public Fermentable Fermentable { get; set; }
        public int ProportionOfGrist { get; set; }

        public MaltCategory Category => (MaltCategory) Fermentable.Characteristics.MaltCategory;
    }
}