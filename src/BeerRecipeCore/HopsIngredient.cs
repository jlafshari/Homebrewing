namespace BeerRecipeCore
{
    public class HopsIngredient
    {
        public HopsIngredient(Hops hopsInfo)
        {
            m_hopsInfo = hopsInfo;
        }

        public int Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public float Amount
        {
            get { return m_amount; }
            set { m_amount = value; }
        }

        public float Time
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

        private int m_id;
        private float m_amount;
        private float m_time;
        private HopsFlavorType m_flavorType;
        private HopsForm m_form;
        private Hops m_hopsInfo;
    }
}
