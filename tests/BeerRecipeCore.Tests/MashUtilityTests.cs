﻿using BeerRecipeCore.Formulas;
using Xunit;

namespace BeerRecipeCore.Tests
{
    public class MashUtilityTests
    {
        [Fact]
        public void GetInitialStrikeTemperatureTest()
        {
            var actualStrikeTemperature = MashUtility.GetInitialStrikeTemperature(1f, 70, 104);
            Assert.Equal(111, actualStrikeTemperature);
        }

        [Fact]
        public void GetMashStepWaterAmountTest()
        {
            var actualMashStepWaterAmount = MashUtility.GetMashStepWaterAmount(104, 140, 8, 8, 210);
            Assert.Equal(4.9f, actualMashStepWaterAmount);
        }
    }
}
