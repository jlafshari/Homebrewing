using Utility;

namespace BeerRecipeCore
{
    public class Yeast : BeerXmlObject
    {
        public Yeast(string name, int version, string notes, string type, string form, float amount, bool amountIsWeight, string laboratory,
            string productId, float minTemperature, float maxTemperature, string flocculation, float attenuation, string bestFor)
            : base(name, version, notes)
        {
            m_type = (YeastType) EnumConverter.Parse(typeof(YeastType), type);
            m_form = (YeastForm) EnumConverter.Parse(typeof(YeastForm), form);
            m_amount = amount;
            m_amountIsWeight = amountIsWeight;
            m_laboratory = laboratory;
            m_productId = productId;
            m_minTemperature = minTemperature;
            m_maxTemperature = maxTemperature;
            m_flocculation = (Flocculation) EnumConverter.Parse(typeof(Flocculation), flocculation);
            m_attenuation = attenuation;
            m_bestFor = bestFor;
        }

        public YeastType Type
        {
            get { return m_type; }
        }

        public YeastForm Form
        {
            get { return m_form; }
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

        public float MinTemperature
        {
            get { return m_minTemperature; }
        }

        public float MaxTemperature
        {
            get { return m_maxTemperature; }
        }

        public Flocculation Flocculation
        {
            get { return m_flocculation; }
        }

        public float Attenuation
        {
            get { return m_attenuation; }
        }

        public string BestFor
        {
            get { return m_bestFor; }
        }

        private YeastType m_type;
        private YeastForm m_form;
        private float m_amount;
        private bool m_amountIsWeight;
        private string m_laboratory;
        private string m_productId;
        private float m_minTemperature;
        private float m_maxTemperature;
        private Flocculation m_flocculation;
        private float m_attenuation;
        private string m_bestFor;
    }
}
