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
            string name = GetNameFromRecord(fermentableEntry);
            string origin = fermentableEntry.Element("ORIGIN").Value;
            string notes = GetNotesFromRecord(fermentableEntry);
            FermentableType type = EnumConverter.Parse<FermentableType>(fermentableEntry.Element("TYPE").Value);
            float yieldValue = (float) Convert.ToDouble(fermentableEntry.Element("YIELD").Value);
            float? yield = type == FermentableType.Grain ? (float?) yieldValue : null;
            float? yieldByWeight = type != FermentableType.Grain ? (float?) yieldValue : null;
            float color = (float) Convert.ToDouble(fermentableEntry.Element("COLOR").Value);
            float diastaticPowerParsed;
            bool diastaticPowerIsntNull = float.TryParse(fermentableEntry.Element("DIASTATIC_POWER").Value, out diastaticPowerParsed);
            float? diastaticPower = diastaticPowerIsntNull ? (float?) diastaticPowerParsed : null;
            double potential = Convert.ToDouble(fermentableEntry.Element("POTENTIAL").Value);
            int gravityUnit = AlcoholUtility.GetGravityUnit(potential);
            FermentableCharacteristics characteristics = new FermentableCharacteristics(yield, color, diastaticPower) { Type = type, YieldByWeight = yieldByWeight, GravityPoint = gravityUnit };
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
