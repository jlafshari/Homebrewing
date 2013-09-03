﻿using Utility;

namespace BeerRecipeCore
{
    public class YeastCharacteristics
    {
        public YeastCharacteristics(string type, string flocculation, string form)
        {
            m_type = (YeastType) EnumConverter.Parse(typeof(YeastType), type);
            m_flocculation = (Flocculation) EnumConverter.Parse(typeof(Flocculation), flocculation);
            m_form = (YeastForm) EnumConverter.Parse(typeof(YeastForm), form);
        }

        public YeastType Type
        {
            get { return m_type; }
        }

        public Flocculation Flocculation
        {
            get { return m_flocculation; }
        }

        public YeastForm Form
        {
            get { return m_form; }
        }

        public float MinTemperature
        {
            get { return m_minTemperature; }
            set { m_minTemperature = value; }
        }

        public float MaxTemperature
        {
            get { return m_maxTemperature; }
            set { m_maxTemperature = value; }
        }

        public float Attenuation
        {
            get { return m_attenuation; }
            set { m_attenuation = value; }
        }

        private YeastType m_type;
        private YeastForm m_form;
        private float m_minTemperature;
        private float m_maxTemperature;
        private Flocculation m_flocculation;
        private float m_attenuation;
    }
}