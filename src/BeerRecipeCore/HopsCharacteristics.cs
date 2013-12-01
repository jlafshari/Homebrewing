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

        /// <summary>
        /// The alpha acid percentage.
        /// </summary>
        public float AlphaAcid
        {
            get { return m_alphaAcid; }
        }

        /// <summary>
        /// The beta acid percentage.
        /// </summary>
        public float BetaAcid
        {
            get { return m_betaAcid; }
        }

        /// <summary>
        /// The Hop Stability Index: the percentage of hop alpha acid lost in 6 months of storage.
        /// </summary>
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
