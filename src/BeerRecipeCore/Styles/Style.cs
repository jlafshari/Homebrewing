using System.Collections.Generic;

namespace BeerRecipeCore.Styles
{
    public record Style(string Name, StyleCategory Category, StyleClassification Classification, List<StyleThreshold> Thresholds)
        : IStyle
    {
        public string Notes { get; init; }

        public string Profile { get; init; }

        public string Ingredients { get; init; }

        public string Examples { get; init; }
        public List<CommonGrain> CommonGrains { get; set; } = new();
    }
}
