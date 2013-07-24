using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerRecipeCore
{
    public class Fermentable : BeerXmlObject
    {
        public Fermentable(string name, int version, string notes, float yield, float color, string origin, float? diastaticPower)
            : base(name, version, notes)
        {
            m_yield = yield;
            m_color = color;
            m_origin = origin;
            m_diastaticPower = diastaticPower;
        }

        public float Yield
        {
            get { return m_yield; }
        }

        public float Color
        {
            get { return m_color; }
        }

        public string Origin
        {
            get { return m_origin; }
        }

        public float? DiastaticPower
        {
            get { return m_diastaticPower; }
        }

        private float m_yield;
        private float m_color;
        private string m_origin = "";
        private float? m_diastaticPower; 
    }
}
