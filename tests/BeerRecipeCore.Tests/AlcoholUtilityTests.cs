﻿using System;
using System.Collections.Generic;
using BeerRecipeCore.Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BeerRecipeCore.Tests
{
    [TestClass]
    public class AlcoholUtilityTests
    {
        [TestMethod]
        public void GetAlcoholByVolumeTest()
        {
            float originalGravity = 1.045f;
            float finalGravity = 1.006f;
            float actualAbv = (float) Math.Round(AlcoholUtility.GetAlcoholByVolume(originalGravity, finalGravity), 2);
            Assert.AreEqual(5.15f, actualAbv);
        }

        [TestMethod]
        public void GetAlcoholByWeightTest()
        {
            float actualAbw = (float) Math.Round(AlcoholUtility.GetAlcoholByWeight(5.12f), 2);
            Assert.AreEqual(4.06f, actualAbw);
        }

        [TestMethod]
        public void GetSpecificGravityTest()
        {
            Fermentable crystal60 = new Fermentable("Caramel/Crystal Malt - 60L", new FermentableCharacteristics(74, 60, 0) { Type = FermentableType.Grain, GravityPoint = 34 }, "Test Notes", "US");
            FermentableIngredient crystal60InRecipe = new FermentableIngredient(crystal60) { Amount = 0.50f };
            Fermentable chocolateMalt = new Fermentable("Chocolate Malt", new FermentableCharacteristics(60, 350, 0) { Type = FermentableType.Grain, GravityPoint = 28 }, "Test Notes", "US");
            FermentableIngredient chocolateMaltInRecipe = new FermentableIngredient(chocolateMalt) { Amount = 1 };
            Fermentable marisOtter = new Fermentable("Pale Malt, Maris Otter", new FermentableCharacteristics(82.5f, 3, 120) { Type = FermentableType.Grain, GravityPoint = 38 }, "Test Notes", "US");
            FermentableIngredient marisOtterInRecipe = new FermentableIngredient(marisOtter) { Amount = 8 };

            List<FermentableIngredient> fermentablesInRecipe = new List<FermentableIngredient>() { crystal60InRecipe, chocolateMaltInRecipe, marisOtterInRecipe };
            float actualSpecificGravity = AlcoholUtility.GetOriginalGravity(fermentablesInRecipe, 5, 70);
            Assert.AreEqual(1.049f, actualSpecificGravity);
        }

        [TestMethod]
        public void GetGravityPointTest()
        {
            double specificGravity = 1.037;
            int gravityPoint = AlcoholUtility.GetGravityPoint(specificGravity);
            Assert.AreEqual(37, gravityPoint);
        }
    }
}
