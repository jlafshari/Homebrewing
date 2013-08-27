namespace BeerRecipeCore
{
    public class FermentableCharacteristics
    {
        public FermentableCharacteristics(float yield, float color, float? diastaticPower)
        {
            m_yield = yield;
            m_color = color;
            m_diastaticPower = diastaticPower;
        }

        public float Yield
        {
            get { return m_yield; }
        }

        public float Color
        {
            get { return m_color; }
        }

        public float? DiastaticPower
        {
            get { return m_diastaticPower; }
        }

        private float m_yield;
        private float m_color;
        private float? m_diastaticPower;
    }
}
