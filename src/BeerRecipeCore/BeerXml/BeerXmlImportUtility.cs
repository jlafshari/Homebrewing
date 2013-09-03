﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BeerRecipeCore.BeerXml
{
    public static class BeerXmlImportUtility
    {
        public static Hops GetHops(XElement hopsEntry)
        {
            string name = GetNameFromRecord(hopsEntry);
            string origin = hopsEntry.Element("ORIGIN").Value;
            float alphaAcid = (float) Convert.ToDouble(hopsEntry.Element("ALPHA").Value);
            float betaAcid = (float) Convert.ToDouble(hopsEntry.Element("BETA").Value);
            string use = hopsEntry.Element("USE").Value;
            string notes = GetNotesFromRecord(hopsEntry);
            float hsi = (float) Convert.ToDouble(hopsEntry.Element("HSI").Value);
            HopsCharacteristics hopsCharacteristics = new HopsCharacteristics(alphaAcid, betaAcid) { Hsi = hsi };
            return new Hops(name, hopsCharacteristics, use, notes, origin);
        }

        public static Fermentable GetFermentable(XElement fermentableEntry)
        {
            string name = GetNameFromRecord(fermentableEntry);
            string origin = fermentableEntry.Element("ORIGIN").Value;
            string notes = GetNotesFromRecord(fermentableEntry);
            float yield = (float) Convert.ToDouble(fermentableEntry.Element("YIELD").Value);
            float color = (float) Convert.ToDouble(fermentableEntry.Element("COLOR").Value);
            float diastaticPowerParsed;
            bool diastaticPowerIsntNull = float.TryParse(fermentableEntry.Element("DIASTATIC_POWER").Value, out diastaticPowerParsed);
            float? diastaticPower = diastaticPowerIsntNull ? (float?) diastaticPowerParsed : null;
            FermentableCharacteristics characteristics = new FermentableCharacteristics(yield, color, diastaticPower);
            return new Fermentable(name, characteristics, notes, origin);
        }

        public static Yeast GetYeast(XElement yeastEntry)
        {
            string name = GetNameFromRecord(yeastEntry);
            string type = yeastEntry.Element("TYPE").Value;
            string form = yeastEntry.Element("FORM").Value;
            float amount = (float) Convert.ToDouble(yeastEntry.Element("AMOUNT").Value);
            bool amountIsWeight = bool.Parse(yeastEntry.Element("AMOUNT_IS_WEIGHT").Value);
            string laboratory = yeastEntry.Element("LABORATORY").Value;
            string productId = yeastEntry.Element("PRODUCT_ID").Value;
            float minTemperature = (float) Convert.ToDouble(yeastEntry.Element("MIN_TEMPERATURE").Value);
            float maxTemperature = (float) Convert.ToDouble(yeastEntry.Element("MAX_TEMPERATURE").Value);
            string flocculation = yeastEntry.Element("FLOCCULATION").Value;
            float attenuation = (float) Convert.ToDouble(yeastEntry.Element("ATTENUATION").Value);
            string notes = GetNotesFromRecord(yeastEntry);

            YeastCharacteristics characteristics = new YeastCharacteristics(type, flocculation, form) { Attenuation = attenuation, MinTemperature = minTemperature, MaxTemperature = maxTemperature };
            return new Yeast(name, characteristics, notes, amount, amountIsWeight, laboratory, productId);
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
            string type = styleEntry.Element("TYPE").Value;
            StyleCategory category = new StyleCategory(categoryName, categoryNumber, type);
            
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