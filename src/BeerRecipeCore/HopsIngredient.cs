﻿namespace BeerRecipeCore
{
    public class HopsIngredient : IHopsIngredient
    {
        public HopsIngredient(Hops hopsInfo)
        {
            m_hopsInfo = hopsInfo;
        }

        /// <summary>
        /// The amount of hops used, in ounces.
        /// </summary>
        public float Amount
        {
            get { return m_amount; }
            set { m_amount = value; }
        }

        /// <summary>
        /// The amount of time the hops is used, in minutes.
        /// </summary>
        public int Time
        {
            get { return m_time; }
            set { m_time = value; }
        }

        public int? DryHopTime
        {
            get { return m_dryHopTime; }
            set { m_dryHopTime = value; }
        }

        public HopsFlavorType FlavorType
        {
            get { return m_flavorType; }
            set { m_flavorType = value; }
        }

        public HopsForm Form
        {
            get { return m_form; }
            set { m_form = value; }
        }

        public HopsUse Use
        {
            get { return m_use; }
            set { m_use = value; }
        }

        public Hops HopsInfo
        {
            get { return m_hopsInfo; }
        }

        private float m_amount;
        private int m_time;
        private int? m_dryHopTime;
        private HopsFlavorType m_flavorType;
        private HopsForm m_form;
        private HopsUse m_use;
        private Hops m_hopsInfo;
    }
}
