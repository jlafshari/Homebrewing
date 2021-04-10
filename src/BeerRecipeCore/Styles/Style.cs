using System.Collections.ObjectModel;

namespace BeerRecipeCore.Styles
{
    public record Style(string Name, StyleCategory Category, StyleClassification Classification, ReadOnlyCollection<StyleThreshold> Thresholds) : IStyle
    {
        public string Notes { get; init; }

        public string Profile { get; init; }

        public string Ingredients { get; init; }

        public string Examples { get; init; }
    }
}
