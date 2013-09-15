using System;
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
    }
}
