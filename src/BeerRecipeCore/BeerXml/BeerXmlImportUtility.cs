﻿using System;
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
            string origin = hopsEntry.GetChildElementValue("ORIGIN");
            float alphaAcid = Convert.ToSingle(hopsEntry.GetChildElementValue("ALPHA"));
            float betaAcid = Convert.ToSingle(hopsEntry.GetChildElementValue("BETA"));
            string notes = GetNotesFromRecord(hopsEntry);
            float hsi = Convert.ToSingle(hopsEntry.GetChildElementValue("HSI"));
            var hopsCharacteristics = new HopsCharacteristics(alphaAcid, betaAcid, hsi);
            return new Hops.Hops(name, hopsCharacteristics, notes, origin);
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
            string name = GetNameFromRecord(yeastEntry);
            string type = yeastEntry.GetChildElementValue("TYPE");
            string form = yeastEntry.GetChildElementValue("FORM");
            string laboratory = yeastEntry.GetChildElementValue("LABORATORY");
            string productId = yeastEntry.GetChildElementValue("PRODUCT_ID");
            float minTemperature = Convert.ToSingle(yeastEntry.GetChildElementValue("MIN_TEMPERATURE"));
            float maxTemperature = Convert.ToSingle(yeastEntry.GetChildElementValue("MAX_TEMPERATURE"));
            string flocculation = yeastEntry.GetChildElementValue("FLOCCULATION");
            float attenuation = Convert.ToSingle(yeastEntry.GetChildElementValue("ATTENUATION"));
            string notes = GetNotesFromRecord(yeastEntry);

            YeastCharacteristics characteristics = new YeastCharacteristics(type, flocculation, form) { Attenuation = attenuation, MinTemperature = minTemperature, MaxTemperature = maxTemperature };
            return new Yeast.Yeast(name, characteristics, notes, laboratory, productId);
        }

        public static Style GetStyle(XElement styleEntry)
        {
            string name = GetNameFromRecord(styleEntry);
            List<StyleThreshold> thresholds = new List<StyleThreshold>()
            {
                new StyleThreshold("og", Convert.ToSingle(styleEntry.GetChildElementValue("OG_MIN")), Convert.ToSingle(styleEntry.GetChildElementValue("OG_MAX"))),
                new StyleThreshold("fg", Convert.ToSingle(styleEntry.GetChildElementValue("FG_MIN")), Convert.ToSingle(styleEntry.GetChildElementValue("FG_MAX"))),
                new StyleThreshold("ibu", Convert.ToSingle(styleEntry.GetChildElementValue("IBU_MIN")), Convert.ToSingle(styleEntry.GetChildElementValue("IBU_MAX"))),
                new StyleThreshold("color", Convert.ToSingle(styleEntry.GetChildElementValue("COLOR_MIN")), Convert.ToSingle(styleEntry.GetChildElementValue("COLOR_MAX"))),
                new StyleThreshold("carb", Convert.ToSingle(styleEntry.GetChildElementValue("CARB_MIN")), Convert.ToSingle(styleEntry.GetChildElementValue("CARB_MAX"))),
                new StyleThreshold("abv", Convert.ToSingle(styleEntry.GetChildElementValue("ABV_MIN")), Convert.ToSingle(styleEntry.GetChildElementValue("ABV_MAX"))),
            };

            string categoryName = styleEntry.GetChildElementValue("CATEGORY");
            int categoryNumber = Convert.ToInt32(styleEntry.GetChildElementValue("CATEGORY_NUMBER"));
            var type = EnumConverter.Parse<StyleType>(styleEntry.GetChildElementValue("TYPE"));
            var category = new StyleCategory(categoryName, categoryNumber, type);
            
            string styleLetter = styleEntry.GetChildElementValue("STYLE_LETTER");
            string styleGuide = styleEntry.GetChildElementValue("STYLE_GUIDE");
            StyleClassification classification = new StyleClassification(styleLetter, styleGuide);

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
            string notes = entry.GetChildElementValue("NOTES");
            notes = notes.Trim();
            notes = notes.Replace("  ", " ");
            return notes;
        }
    }
}
