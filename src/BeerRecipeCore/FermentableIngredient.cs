namespace BeerRecipeCore
{
    public class FermentableIngredient
    {
        public FermentableIngredient(Fermentable fermentableInfo)
        {
            m_fermentableInfo = fermentableInfo;
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

        public Fermentable FermentableInfo
        {
            get { return m_fermentableInfo; }
        }

        public FermentableType Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        private float m_amount;
        private float m_time;
        private Fermentable m_fermentableInfo;
        private FermentableType m_type;
    }
}
