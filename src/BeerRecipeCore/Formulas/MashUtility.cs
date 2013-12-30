using System;

namespace BeerRecipeCore.Formulas
{
    public static class MashUtility
    {
        /// <summary>
        /// Gets the initial strike temperature for water in degrees Fahrenheit.
        /// </summary>
        /// <param name="waterToGrainRatio">The ratio of water to grain in quarts per pound.</param>
        /// <param name="grainTemperature">Grain temperature in degrees Fahrenheit.</param>
        /// <param name="targetTemperature">Target temperature in degrees Fahrenheit.</param>
        public static int GetInitialStrikeTemperature(float waterToGrainRatio, int grainTemperature, int targetTemperature)
        {
            double strikeTemperature = (c_thermodynamicConstant / waterToGrainRatio) * (targetTemperature - grainTemperature) + targetTemperature;
            return (int) Math.Round(strikeTemperature);
        }

        /// <summary>
        /// Gets the amount of water in quarts necessary to raise the mash temperature to the specified step.
        /// </summary>
        /// <param name="initialTemperature">Initial temperature of the mash in degrees Fahrenheit.</param>
        /// <param name="targetTemperature">Target temperature of the mash in degrees Fahrenheit.</param>
        /// <param name="grainAmount">Amount of grains in the mash in pounds.</param>
        /// <param name="waterAmountInMash">Total amount of water in the mash in quarts.</param>
        /// <param name="infusionWaterTemperature">Temperature of the infusion water in degrees Fahrenheit.</param>
        public static float GetMashStepWaterAmount(int initialTemperature, int targetTemperature, float grainAmount, float waterAmountInMash, int infusionWaterTemperature)
        {
            double waterAmount = (targetTemperature - initialTemperature) * (c_thermodynamicConstant * grainAmount + waterAmountInMash) /
                (infusionWaterTemperature - targetTemperature);
            return (float) Math.Round(waterAmount, 1);
        }

        private const float c_thermodynamicConstant = 0.2f;
    }
}
