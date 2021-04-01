namespace BeerRecipeCore.Fermentables
{
    public class FermentableIngredient : IFermentableIngredient
    {
        public FermentableIngredient(Fermentable fermentableInfo)
        {
            m_fermentableInfo = fermentableInfo;
        }

        /// <summary>
        /// The amount of fermentable used, in pounds.
        /// </summary>
        public float Amount
        {
            get { return m_amount; }
            set { m_amount = value; }
        }

        public Fermentable FermentableInfo
        {
            get { return m_fermentableInfo; }
        }

        private float m_amount;
        private Fermentable m_fermentableInfo;
    }
}
