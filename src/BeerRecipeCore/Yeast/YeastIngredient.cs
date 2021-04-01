namespace BeerRecipeCore.Yeast
{
    public class YeastIngredient : IYeastIngredient
    {
        public YeastIngredient(Yeast yeastInfo)
        {
            m_yeastInfo = yeastInfo;
        }

        public float Weight
        {
            get { return m_weight; }
            set { m_weight = value; }
        }

        public float Volume
        {
            get { return m_volume; }
            set { m_volume = value; }
        }

        public Yeast YeastInfo
        {
            get { return m_yeastInfo; }
            set { m_yeastInfo = value; }
        }

        private float m_weight;
        private float m_volume;
        private Yeast m_yeastInfo;
    }
}
