using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BeerRecipeCore.Styles
{
    public class Style
    {
        public Style(string name, StyleCategory category, StyleClassification classification, IList<StyleThreshold> thresholds)
        {
            Name = name;
            Category = category;
            Classification = classification;
            Thresholds = new ReadOnlyCollection<StyleThreshold>(thresholds);
        }

        public string Name { get; }

        public string Notes { get; set; }

        public StyleCategory Category { get; }

        public StyleClassification Classification { get; }

        public string Profile { get; set; }

        public string Ingredients { get; set; }

        public string Examples { get; set; }

        public ReadOnlyCollection<StyleThreshold> Thresholds { get; }
    }
}
