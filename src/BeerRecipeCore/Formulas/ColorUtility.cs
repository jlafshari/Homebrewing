using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using BeerRecipeCore.Fermentables;

namespace BeerRecipeCore.Formulas
{
    public static class ColorUtility
    {
        public static double GetColorInSrm(IEnumerable<IFermentableIngredient> fermentables, float recipeVolumeInGallons)
        {
            // calculate MCU from each fermentable
            double totalMcu = 0;
            foreach (IFermentableIngredient fermentable in fermentables)
            {
                double fermentableMcu = GetMaltColorUnit(fermentable, recipeVolumeInGallons);
                totalMcu += fermentableMcu;
            }
            
            // get SRM color using Morey equation
            return Math.Round(1.4922 * Math.Pow(totalMcu, 0.6859), 1);
        }

        public static Color GetColorFromSrm(double srmColor)
        {
            if (srmColor < 0)
                throw new ArgumentException("srmColor must be a positive number");

            int srmColorWholeNumber = (int) Math.Round(srmColor);
            if (s_srmColors.ContainsKey(srmColorWholeNumber))
                return s_srmColors[srmColorWholeNumber];
            else if (srmColorWholeNumber > c_darkestSupportedSrm)
                return s_srmColors[c_darkestSupportedSrm];
            else
                return s_srmColors[1];
        }

        private static double GetMaltColorUnit(IFermentableIngredient fermentable, float recipeVolumeInGallons)
        {
            return fermentable.FermentableInfo.Characteristics.Color * fermentable.Amount / recipeVolumeInGallons;
        }

        const int c_darkestSupportedSrm = 30;
        static readonly Dictionary<int, Color> s_srmColors = new Dictionary<int, Color>
        {
            { 1, Color.FromRgb(243, 249, 147) },
            { 2, Color.FromRgb(245, 247, 92) },
            { 3, Color.FromRgb(246, 245, 19) },
            { 4, Color.FromRgb(234, 230, 21) },
            { 5, Color.FromRgb(224, 208, 27) },
            { 6, Color.FromRgb(213, 188, 38) },
            { 7, Color.FromRgb(205, 170, 55) },
            { 8, Color.FromRgb(193, 150, 60) },
            { 9, Color.FromRgb(190, 140, 58) },
            { 10, Color.FromRgb(190, 130, 58) },
            { 11, Color.FromRgb(193, 122, 55) },
            { 12, Color.FromRgb(191, 113, 56) },
            { 13, Color.FromRgb(188, 103, 51) },
            { 14, Color.FromRgb(178, 96, 51) },
            { 15, Color.FromRgb(168, 88, 57) },
            { 16, Color.FromRgb(152, 83, 54) },
            { 17, Color.FromRgb(141, 76, 50) },
            { 18, Color.FromRgb(124, 69, 45) },
            { 19, Color.FromRgb(107, 58, 30) },
            { 20, Color.FromRgb(93, 52, 26) },
            { 21, Color.FromRgb(78, 42, 12) },
            { 22, Color.FromRgb(74, 39, 39) },
            { 23, Color.FromRgb(54, 31, 27) },
            { 24, Color.FromRgb(38, 23, 22) },
            { 25, Color.FromRgb(35, 23, 22) },
            { 26, Color.FromRgb(25, 16, 15) },
            { 27, Color.FromRgb(22, 16, 15) },
            { 28, Color.FromRgb(18, 13, 12) },
            { 29, Color.FromRgb(16, 11, 10) },
            { 30, Color.FromRgb(5, 11, 10) },
        };
    }
}
