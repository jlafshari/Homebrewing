using Utility;

namespace BeerRecipeCore.Hops
{
    public class Hops : IngredientTypeBase
    {
        public Hops(string name, HopsCharacteristics characteristics, string notes, string origin)
            : base(name, notes)
        {
            m_characteristics = characteristics;
            m_origin = origin;
        }

        public HopsCharacteristics Characteristics
        {
            get { return m_characteristics; }
        }

        public string Origin
        {
            get { return m_origin; }
        }

        private string m_origin = "";
        private HopsCharacteristics m_characteristics;
    }
}
