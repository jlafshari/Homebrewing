using System.Collections.ObjectModel;

namespace BeerRecipeCore.Mash
{
    public class MashProfile
    {
        public MashProfile(int grainStartingTemperature)
        {
            GrainStartingTemperature = grainStartingTemperature;
        }

        public Collection<MashStep> MashSteps { get; private set; } = new();

        /// <summary>
        /// The initial temperature of the grain, in degrees Fahrenheit.
        /// </summary>
        public int GrainStartingTemperature { get; set; }

        /// <summary>
        /// The water to grain ratio at the first mash step, in quarts per pound.
        /// </summary>
        public float WaterToGrainRatio { get; set; }
    }
}
