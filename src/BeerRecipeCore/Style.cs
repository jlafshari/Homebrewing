using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BeerRecipeCore
{
    public class Style
    {
        public Style(string name, StyleCategory category, StyleClassification classification, IList<StyleThreshold> thresholds)
        {
            m_name = name;
            m_category = category;
            m_classification = classification;
            m_thresholds = new ReadOnlyCollection<StyleThreshold>(thresholds);
        }

        public string Name
        {
            get { return m_name; }
        }

        public string Notes
        {
            get { return m_notes; }
            set { m_notes = value; }
        }

        public StyleCategory Category
        {
            get { return m_category; }
        }

        public StyleClassification Classification
        {
            get { return m_classification; }
        }

        public string Profile
        {
            get { return m_profile; }
            set { m_profile = value; }
        }

        public string Ingredients
        {
            get { return m_ingredients; }
            set { m_ingredients = value; }
        }

        public string Examples
        {
            get { return m_examples; }
            set { m_examples = value; }
        }

        public ReadOnlyCollection<StyleThreshold> Thresholds
        {
            get { return m_thresholds; }
        }

        private string m_name = "";
        private string m_notes = "";
        private StyleCategory m_category;
        private StyleClassification m_classification;
        private string m_profile = "";
        private string m_ingredients = "";
        private string m_examples = "";
        private ReadOnlyCollection<StyleThreshold> m_thresholds;
    }
}
