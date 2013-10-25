﻿using System.Collections.Generic;
using BeerRecipeCore.Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BeerRecipeCore.Tests
{
    [TestClass]
    public class BitternessUtilityTests
    {
        [TestMethod]
        public void GetBitternessTest()
        {
            Hops fuggles = new Hops("Fuggles", new HopsCharacteristics(4.50f, 2.00f), "Boil", "Test Notes", "UK");
            HopsIngredient fugglesInRecipe = new HopsIngredient(fuggles) { Amount = 1f, Form = HopsForm.Leaf, FlavorType = HopsFlavorType.Bittering, Time = 60f };
            
            Hops goldings = new Hops("Goldings", new HopsCharacteristics(5.00f, 3.50f), "Boil", "Test Notes", "UK");
            HopsIngredient goldingsInRecipe = new HopsIngredient(goldings) { Amount = 1f, Form = HopsForm.Pellet, FlavorType = HopsFlavorType.Aroma, Time = 15f };

            List<HopsIngredient> hopsUsed = new List<HopsIngredient>() { fugglesInRecipe, goldingsInRecipe };
            int bitterness = BitternessUtility.GetBitterness(hopsUsed, 5f, 1.054f);
            Assert.AreEqual(23, bitterness);
        }
    }
}