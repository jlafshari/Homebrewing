namespace BeerRecipeCore
{
    public class FermentableCharacteristics
    {
        public FermentableCharacteristics(float? yield, float color, float? diastaticPower)
        {
            m_yield = yield;
            m_color = color;
            m_diastaticPower = diastaticPower;
            m_yieldByWeight = null;
        }

        /// <summary>
        /// The percent dry yield if the fermentable is a grain.
        /// </summary>
        public float? Yield
        {
            get { return m_yield; }
        }

        /// <summary>
        /// The percent raw yield by weight if this is an extract, adjunct, or sugar.
        /// </summary>
        public float? YieldByWeight
        {
            get { return m_yieldByWeight; }
            set { m_yieldByWeight = value; }
        }

        public double Color
        {
            get { return m_color; }
        }

        /// <summary>
        /// The diastatic power in Lintner degree units. If the FermentableType is not a grain or adjunct, then this value is null.
        /// </summary>
        public float? DiastaticPower
        {
            get { return m_diastaticPower; }
        }

        public FermentableType Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        public int GravityPoint
        {
            get { return m_gravityPoint; }
            set { m_gravityPoint = value; }
        }

        private float? m_yield;
        private float? m_yieldByWeight;
        private double m_color;
        private float? m_diastaticPower;
        private FermentableType m_type;
        private int m_gravityPoint;
    }
}
