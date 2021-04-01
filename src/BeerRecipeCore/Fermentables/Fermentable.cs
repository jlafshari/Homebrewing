namespace BeerRecipeCore.Fermentables
{
    public class Fermentable : IngredientTypeBase
    {
        public Fermentable(string name, FermentableCharacteristics characteristics, string notes, string origin)
            : base(name, notes)
        {
            m_characteristics = characteristics;
            m_origin = origin;
        }

        public FermentableCharacteristics Characteristics
        {
            get { return m_characteristics; }
        }

        public string Origin
        {
            get { return m_origin; }
        }

        private string m_origin = "";
        private FermentableCharacteristics m_characteristics;
    }
}
