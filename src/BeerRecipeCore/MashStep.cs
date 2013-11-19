namespace BeerRecipeCore
{
    public class MashStep
    {
        public MashStep(MashType type)
        {
            m_type = type;

            // decoction mash steps don't have any infusion water
            if (m_type == MashType.Decoction)
                m_infusionWaterTemperature = null;
        }

        public MashType Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        /// <summary>
        /// The amount of water to add to the mash, in quarts.
        /// </summary>
        public float InfuseAmount
        {
            get { return m_infuseAmount; }
            set { m_infuseAmount = value; }
        }

        /// <summary>
        /// The target temperature, in degrees Fahrenheit.
        /// </summary>
        public int TargetTemperature
        {
            get { return m_targetTemperature; }
            set { m_targetTemperature = value; }
        }

        /// <summary>
        /// The duration of the mash step in minutes.
        /// </summary>
        public int Duration
        {
            get { return m_duration; }
            set { m_duration = value; }
        }

        /// <summary>
        /// The infusion water temperature, in degrees Fahrenheit.
        /// </summary>
        public int? InfusionWaterTemperature
        {
            get { return m_infusionWaterTemperature; }
            set { m_infusionWaterTemperature = value; }
        }

        private MashType m_type;
        private float m_infuseAmount;
        private int m_targetTemperature;
        private int m_duration;
        private int? m_infusionWaterTemperature;
    }
}
