using System.Collections.Generic;

namespace BeerRecipeCore.Styles
{
    public interface IStyle
    {
        public string Name { get; }
        public StyleCategory Category { get; }
        public StyleClassification Classification { get; }
        public List<StyleThreshold> Thresholds { get; }
        public List<CommonGrain> CommonGrains { get; set; }
        public List<CommonHop> CommonHops { get; set; }
        public Yeast.Yeast CommonYeast { get; set; }
    }
}