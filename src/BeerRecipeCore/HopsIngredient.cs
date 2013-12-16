namespace BeerRecipeCore
{
    public class HopsIngredient : IHopsIngredient
    {
        public HopsIngredient(Hops hopsInfo)
        {
            m_hopsInfo = hopsInfo;
        }

        /// <summary>
        /// The amount of hops used, in ounces.
        /// </summary>
        public float Amount
        {
            get { return m_amount; }
            set { m_amount = value; }
        }

        /// <summary>
        /// The amount of time the hops is used, in minutes.
        /// </summary>
        public int Time
        {
            get { return m_time; }
            set { m_time = value; }
        }

        public HopsFlavorType FlavorType
        {
            get { return m_flavorType; }
            set { m_flavorType = value; }
        }

        public HopsForm Form
        {
            get { return m_form; }
            set { m_form = value; }
        }

        public Hops HopsInfo
        {
            get { return m_hopsInfo; }
        }

        private float m_amount;
        private int m_time;
        private HopsFlavorType m_flavorType;
        private HopsForm m_form;
        private Hops m_hopsInfo;
    }
}
