using Utility;

namespace BeerRecipeCore
{
    public class Yeast : BeerXmlObject
    {
        public Yeast(string name, YeastCharacteristics characteristics, string notes, float amount, bool amountIsWeight, string laboratory,
            string productId)
            : base(name, notes)
        {
            m_characteristics = characteristics;
            m_amount = amount;
            m_amountIsWeight = amountIsWeight;
            m_laboratory = laboratory;
            m_productId = productId;
        }

        public YeastCharacteristics Characteristics
        {
            get { return m_characteristics; }
        }

        public float Amount
        {
            get { return m_amount; }
        }

        public bool AmountIsWeight
        {
            get { return m_amountIsWeight ; }
        }

        public string Laboratory
        {
            get { return m_laboratory; }
        }

        public string ProductId
        {
            get { return m_productId; }
        }

        private float m_amount;
        private bool m_amountIsWeight;
        private string m_laboratory;
        private string m_productId;
        private YeastCharacteristics m_characteristics;
    }
}
