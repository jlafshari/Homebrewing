using System.Collections.ObjectModel;

namespace BeerRecipeCore
{
    public class MashProfile
    {
        public MashProfile(int grainStartingTemperature)
        {
            m_grainStartingTemperature = grainStartingTemperature;
        }

        public Collection<MashStep> MashSteps
        {
            get { return m_mashSteps; }
            private set { m_mashSteps = value; }
        }

        /// <summary>
        /// The initial temperature of the grain, in degrees Fahrenheit.
        /// </summary>
        public int GrainStartingTemperature
        {
            get { return m_grainStartingTemperature; }
            set { m_grainStartingTemperature = value; }
        }

        /// <summary>
        /// The water to grain ratio at the first mash step, in quarts per pound.
        /// </summary>
        public float WaterToGrainRatio
        {
            get { return m_waterToGrainRatio; }
            set { m_waterToGrainRatio = value; }
        }

        private Collection<MashStep> m_mashSteps = new Collection<MashStep>();
        private int m_grainStartingTemperature;
        private float m_waterToGrainRatio;
    }
}
