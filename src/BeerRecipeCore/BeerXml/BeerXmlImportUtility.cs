using System;
using System.Collections.Generic;
using System.Xml.Linq;
using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Formulas;
using BeerRecipeCore.Hops;
using BeerRecipeCore.Styles;
using BeerRecipeCore.Yeast;
using Utility;

namespace BeerRecipeCore.BeerXml
{
    public static class BeerXmlImportUtility
    {
        public static Hops.Hops GetHops(XElement hopsEntry)
        {
            string name = GetNameFromRecord(hopsEntry);
            string origin = hopsEntry.Element("ORIGIN").Value;
            float alphaAcid = (float) Convert.ToDouble(hopsEntry.Element("ALPHA").Value);
            float betaAcid = (float) Convert.ToDouble(hopsEntry.Element("BETA").Value);
            string notes = GetNotesFromRecord(hopsEntry);
            float hsi = (float) Convert.ToDouble(hopsEntry.Element("HSI").Value);
            var hopsCharacteristics = new HopsCharacteristics(alphaAcid, betaAcid, hsi);
            return new Hops.Hops(name, hopsCharacteristics, notes, origin);
        }

        public static Fermentable GetFermentable(XElement fermentableEntry)
        {
            var name = GetNameFromRecord(fermentableEntry);
            var origin = fermentableEntry.Element("ORIGIN").Value;
            var notes = GetNotesFromRecord(fermentableEntry);
            var type = EnumConverter.Parse<FermentableType>(fermentableEntry.Element("TYPE").Value);
            var yieldValue = (float) Convert.ToDouble(fermentableEntry.Element("YIELD").Value);
            var yield = type == FermentableType.Grain ? (float?) yieldValue : null;
            var color = (float) Convert.ToDouble(fermentableEntry.Element("COLOR").Value);
            var diastaticPower = float.TryParse(fermentableEntry.Element("DIASTATIC_POWER").Value, out var diastaticPowerParsed) ? (float?) diastaticPowerParsed : null;
            var potential = Convert.ToDouble(fermentableEntry.Element("POTENTIAL").Value);
            var maltCategoryValue = fermentableEntry.Element("malt-category")?.Value;
            var characteristics = new FermentableCharacteristics(yield, color, diastaticPower)
            {
                Type = type,
                YieldByWeight = type != FermentableType.Grain ? yieldValue : null,
                GravityPoint = AlcoholUtility.GetGravityUnit(potential),
                MaltCategory = maltCategoryValue != null ? EnumConverter.Parse<MaltCategory>(maltCategoryValue) : null
            };
            return new Fermentable(name, characteristics, notes, origin);
        }

        public static Yeast.Yeast GetYeast(XElement yeastEntry)
        {
            string name = GetNameFromRecord(yeastEntry);
            string type = yeastEntry.Element("TYPE").Value;
            string form = yeastEntry.Element("FORM").Value;
            string laboratory = yeastEntry.Element("LABORATORY").Value;
            string productId = yeastEntry.Element("PRODUCT_ID").Value;
            float minTemperature = (float) Convert.ToDouble(yeastEntry.Element("MIN_TEMPERATURE").Value);
            float maxTemperature = (float) Convert.ToDouble(yeastEntry.Element("MAX_TEMPERATURE").Value);
            string flocculation = yeastEntry.Element("FLOCCULATION").Value;
            float attenuation = (float) Convert.ToDouble(yeastEntry.Element("ATTENUATION").Value);
            string notes = GetNotesFromRecord(yeastEntry);

            YeastCharacteristics characteristics = new YeastCharacteristics(type, flocculation, form) { Attenuation = attenuation, MinTemperature = minTemperature, MaxTemperature = maxTemperature };
            return new Yeast.Yeast(name, characteristics, notes, laboratory, productId);
        }

        public static Style GetStyle(XElement styleEntry)
        {
            string name = GetNameFromRecord(styleEntry);
            List<StyleThreshold> thresholds = new List<StyleThreshold>()
            {
                new StyleThreshold("og", (float) Convert.ToDouble(styleEntry.Element("OG_MIN").Value), (float) Convert.ToDouble(styleEntry.Element("OG_MAX").Value)),
                new StyleThreshold("fg", (float) Convert.ToDouble(styleEntry.Element("FG_MIN").Value), (float) Convert.ToDouble(styleEntry.Element("FG_MAX").Value)),
                new StyleThreshold("ibu", (float) Convert.ToDouble(styleEntry.Element("IBU_MIN").Value), (float) Convert.ToDouble(styleEntry.Element("IBU_MAX").Value)),
                new StyleThreshold("color", (float) Convert.ToDouble(styleEntry.Element("COLOR_MIN").Value), (float) Convert.ToDouble(styleEntry.Element("COLOR_MAX").Value)),
                new StyleThreshold("carb", (float) Convert.ToDouble(styleEntry.Element("CARB_MIN").Value), (float) Convert.ToDouble(styleEntry.Element("CARB_MAX").Value)),
                new StyleThreshold("abv", (float) Convert.ToDouble(styleEntry.Element("ABV_MIN").Value), (float) Convert.ToDouble(styleEntry.Element("ABV_MAX").Value)),
            };

            string categoryName = styleEntry.Element("CATEGORY").Value;
            int categoryNumber = Convert.ToInt32(styleEntry.Element("CATEGORY_NUMBER").Value);
            var type = EnumConverter.Parse<StyleType>(styleEntry.Element("TYPE").Value);
            var category = new StyleCategory(categoryName, categoryNumber, type);
            
            string styleLetter = styleEntry.Element("STYLE_LETTER").Value;
            string styleGuide = styleEntry.Element("STYLE_GUIDE").Value;
            StyleClassification classification = new StyleClassification(styleLetter, styleGuide);

            return new Style(name, category, classification, thresholds)
            {
                Notes = GetNotesFromRecord(styleEntry),
                Profile = styleEntry.Element("PROFILE").Value,
                Ingredients = styleEntry.Element("INGREDIENTS").Value,
                Examples = styleEntry.Element("EXAMPLES").Value
            };
        }

        private static string GetNameFromRecord(XElement entry)
        {
            return entry.Element("NAME").Value;
        }

        private static string GetNotesFromRecord(XElement entry)
        {
            string notes = entry.Element("NOTES").Value;
            notes = notes.Trim();
            notes = notes.Replace("  ", " ");
            return notes;
        }
    }
}
