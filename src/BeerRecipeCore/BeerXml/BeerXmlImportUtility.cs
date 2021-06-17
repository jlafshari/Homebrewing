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
        public static Hop GetHop(XElement hopEntry)
        {
            var name = GetNameFromRecord(hopEntry);
            var origin = hopEntry.GetChildElementValue("ORIGIN");
            var alphaAcid = Convert.ToSingle(hopEntry.GetChildElementValue("ALPHA"));
            var betaAcid = Convert.ToSingle(hopEntry.GetChildElementValue("BETA"));
            var notes = GetNotesFromRecord(hopEntry);
            var hsi = Convert.ToSingle(hopEntry.GetChildElementValue("HSI"));
            var hopCharacteristics = new HopCharacteristics(alphaAcid, betaAcid, hsi);
            return new Hop(name, hopCharacteristics, notes, origin);
        }

        public static Fermentable GetFermentable(XElement fermentableEntry)
        {
            var name = GetNameFromRecord(fermentableEntry);
            var origin = fermentableEntry.GetChildElementValue("ORIGIN");
            var notes = GetNotesFromRecord(fermentableEntry);
            var type = EnumConverter.Parse<FermentableType>(fermentableEntry.GetChildElementValue("TYPE"));
            var yieldValue = Convert.ToSingle(fermentableEntry.GetChildElementValue("YIELD"));
            var yield = type == FermentableType.Grain ? (float?) yieldValue : null;
            var color = Convert.ToSingle(fermentableEntry.GetChildElementValue("COLOR"));
            var diastaticPower = float.TryParse(fermentableEntry.GetChildElementValue("DIASTATIC_POWER"), out var diastaticPowerParsed) ? (float?) diastaticPowerParsed : null;
            var potential = Convert.ToDouble(fermentableEntry.GetChildElementValue("POTENTIAL"));
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
            var name = GetNameFromRecord(yeastEntry);
            var type = EnumConverter.Parse<YeastType>(yeastEntry.GetChildElementValue("TYPE"));
            var form = EnumConverter.Parse<YeastForm>(yeastEntry.GetChildElementValue("FORM"));
            var laboratory = yeastEntry.GetChildElementValue("LABORATORY");
            var productId = yeastEntry.GetChildElementValue("PRODUCT_ID");
            var minTemperature = Convert.ToSingle(yeastEntry.GetChildElementValue("MIN_TEMPERATURE"));
            var maxTemperature = Convert.ToSingle(yeastEntry.GetChildElementValue("MAX_TEMPERATURE"));
            var flocculation = EnumConverter.Parse<Flocculation>(yeastEntry.GetChildElementValue("FLOCCULATION"));
            var attenuation = Convert.ToSingle(yeastEntry.GetChildElementValue("ATTENUATION"));
            var notes = GetNotesFromRecord(yeastEntry);

            var characteristics = new YeastCharacteristics(type, flocculation, form, minTemperature, maxTemperature, attenuation);
            return new Yeast.Yeast(name, characteristics, notes, laboratory, productId);
        }

        public static Style GetStyle(XElement styleEntry)
        {
            var name = GetNameFromRecord(styleEntry);
            var thresholds = new List<StyleThreshold>
            {
                new("og", Convert.ToSingle(styleEntry.GetChildElementValue("OG_MIN")), Convert.ToSingle(styleEntry.GetChildElementValue("OG_MAX"))),
                new("fg", Convert.ToSingle(styleEntry.GetChildElementValue("FG_MIN")), Convert.ToSingle(styleEntry.GetChildElementValue("FG_MAX"))),
                new("ibu", Convert.ToSingle(styleEntry.GetChildElementValue("IBU_MIN")), Convert.ToSingle(styleEntry.GetChildElementValue("IBU_MAX"))),
                new("color", Convert.ToSingle(styleEntry.GetChildElementValue("COLOR_MIN")), Convert.ToSingle(styleEntry.GetChildElementValue("COLOR_MAX"))),
                new("carb", Convert.ToSingle(styleEntry.GetChildElementValue("CARB_MIN")), Convert.ToSingle(styleEntry.GetChildElementValue("CARB_MAX"))),
                new("abv", Convert.ToSingle(styleEntry.GetChildElementValue("ABV_MIN")), Convert.ToSingle(styleEntry.GetChildElementValue("ABV_MAX")))
            };

            var categoryName = styleEntry.GetChildElementValue("CATEGORY");
            var categoryNumber = Convert.ToInt32(styleEntry.GetChildElementValue("CATEGORY_NUMBER"));
            var type = EnumConverter.Parse<StyleType>(styleEntry.GetChildElementValue("TYPE"));
            var category = new StyleCategory(categoryName, categoryNumber, type);
            
            var styleLetter = styleEntry.GetChildElementValue("STYLE_LETTER");
            var styleGuide = styleEntry.GetChildElementValue("STYLE_GUIDE");
            var classification = new StyleClassification(styleLetter, styleGuide);

            return new Style(name, category, classification, thresholds)
            {
                Notes = GetNotesFromRecord(styleEntry),
                Profile = styleEntry.GetChildElementValue("PROFILE"),
                Ingredients = styleEntry.GetChildElementValue("INGREDIENTS"),
                Examples = styleEntry.GetChildElementValue("EXAMPLES")
            };
        }

        private static string GetNameFromRecord(XElement entry)
        {
            return entry.GetChildElementValue("NAME");
        }

        private static string GetNotesFromRecord(XElement entry)
        {
            var notes = entry.GetChildElementValue("NOTES");
            notes = notes.Trim();
            notes = notes.Replace("  ", " ");
            return notes;
        }
    }
}
