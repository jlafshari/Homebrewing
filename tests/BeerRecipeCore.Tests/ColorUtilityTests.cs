using System;
using System.Collections.Generic;
using BeerRecipeCore.Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BeerRecipeCore.Tests
{
    [TestClass]
    public class ColorUtilityTests
    {
        [TestMethod]
        public void GetColorInSrmTest()
        {
            Fermentable crystal60 = new Fermentable("Caramel/Crystal Malt - 60L", new FermentableCharacteristics(74, 60, 0) { Type = FermentableType.Grain }, "Test Notes", "US");
            FermentableIngredient crystal60InRecipe = new FermentableIngredient(crystal60) { Amount = 0.50f };

            double crystal60Color = Math.Round(ColorUtility.GetColorInSrm(new[] { crystal60InRecipe }, 5), 1);
            Assert.AreEqual(5.1, crystal60Color);

            Fermentable marisOtter = new Fermentable("Pale Malt, Maris Otter", new FermentableCharacteristics(82.5f, 3, 120) { Type = FermentableType.Grain }, "Test Notes", "US");
            FermentableIngredient marisOtterInRecipe = new FermentableIngredient(marisOtter) { Amount = 8 };

            List<FermentableIngredient> fermentablesUsed = new List<FermentableIngredient>() { marisOtterInRecipe, crystal60InRecipe };
            double colorInSrm = Math.Round(ColorUtility.GetColorInSrm(fermentablesUsed, 5), 1);
            Assert.AreEqual(7.6, colorInSrm);
        }
    }
}
