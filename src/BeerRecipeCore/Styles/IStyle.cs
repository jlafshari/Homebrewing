using System.Collections.ObjectModel;

namespace BeerRecipeCore.Styles
{
    public interface IStyle
    {
        public string Name { get; }
        public StyleCategory Category { get; }
        public StyleClassification Classification { get; }
        public ReadOnlyCollection<StyleThreshold> Thresholds { get; }
    }
}