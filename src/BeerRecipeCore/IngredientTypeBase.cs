namespace BeerRecipeCore
{
    public abstract class IngredientTypeBase
    {
        public IngredientTypeBase(string name, string notes)
        {
            m_name = name;
            m_notes = notes;
        }

        public string Name
        {
            get { return m_name; }
        }

        public string Notes
        {
            get { return m_notes; }
        }

        private string m_name = "";
        private string m_notes = "";
    }
}
