using BeerRecipeCore.Formulas;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BeerRecipeCore.Tests
{
    [TestClass]
    public class MashUtilityTests
    {
        [TestMethod]
        public void GetInitialStrikeTemperatureTest()
        {
            int actualStrikeTemperature = MashUtility.GetInitialStrikeTemperature(1f, 70, 104);
            Assert.AreEqual(111, actualStrikeTemperature);
        }

        [TestMethod]
        public void GetMashStepWaterAmountTest()
        {
            float actualMashStepWaterAmount = MashUtility.GetMashStepWaterAmount(104, 140, 8, 8, 210);
            Assert.AreEqual(4.9f, actualMashStepWaterAmount);
        }
    }
}
