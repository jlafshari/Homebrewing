using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerRecipeCore
{
    public class HopsCharacteristics
    {
        public HopsCharacteristics(float alphaAcid, float betaAcid)
        {
            m_alphaAcid = alphaAcid;
            m_betaAcid = betaAcid;
        }

        public float AlphaAcid
        {
            get { return m_alphaAcid; }
        }

        public float BetaAcid
        {
            get { return m_betaAcid; }
        }

        public float Hsi
        {
            get { return m_hsi; }
            set { m_hsi = value; }
        }

        private float m_alphaAcid;
        private float m_betaAcid;
        private float m_hsi;
    }
}
