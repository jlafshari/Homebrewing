using Utility;

namespace BeerRecipeCore
{
    public class Hops : BeerXmlObject
    {
        public Hops(string name, int version, float alphaAcid, float betaAcid, string use, string notes, float hsi, string origin)
            : base(name, version, notes)
        {
            m_alphaAcid = alphaAcid;
            m_betaAcid = betaAcid;
            m_use = (HopsUse) EnumConverter.Parse(typeof(HopsUse), use);
            m_hsi = hsi;
            m_origin = origin;
        }
        
        public float AlphaAcid
        {
            get { return m_alphaAcid; }
        }

        public float BetaAcid
        {
            get { return m_betaAcid; }
        }

        public HopsUse Use
        {
            get { return m_use; }
        }

        public float Hsi
        {
            get { return m_hsi; }
        }

        public string Origin
        {
            get { return m_origin; }
        }

        private float m_alphaAcid;
        private float m_betaAcid;
        private HopsUse m_use;
        private float m_hsi;
        private string m_origin = "";
    }
}
