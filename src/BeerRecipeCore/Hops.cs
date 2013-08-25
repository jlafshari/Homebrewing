using Utility;

namespace BeerRecipeCore
{
    public class Hops : BeerXmlObject
    {
        public Hops(string name, HopsCharacteristics characteristics, int version, string use, string notes, string origin)
            : base(name, version, notes)
        {
            m_characteristics = characteristics;
            m_use = (HopsUse) EnumConverter.Parse(typeof(HopsUse), use);
            m_origin = origin;
        }

        public HopsCharacteristics Characteristics
        {
            get { return m_characteristics; }
        }

        public HopsUse Use
        {
            get { return m_use; }
        }

        public string Origin
        {
            get { return m_origin; }
        }

        private HopsUse m_use;
        private string m_origin = "";
        private HopsCharacteristics m_characteristics;
    }
}
