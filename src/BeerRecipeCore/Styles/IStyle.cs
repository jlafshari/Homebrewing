using System.Collections.Generic;

namespace BeerRecipeCore.Styles
{
    public interface IStyle
    {
        public string Name { get; }
        public StyleCategory Category { get; }
        public StyleClassification Classification { get; }
        public List<StyleThreshold> Thresholds { get; }
    }
}