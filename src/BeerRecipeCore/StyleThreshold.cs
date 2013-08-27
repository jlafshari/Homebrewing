using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerRecipeCore
{
    public class StyleThreshold
    {
        public StyleThreshold(string value, float minimum, float maximum)
        {
            m_value = value;
            m_minimum = minimum;
            m_maximum = maximum;
        }

        public string Value
        {
            get { return m_value; }
        }

        public float Minimum
        {
            get { return m_minimum; }
        }

        public float Maximum
        {
            get { return m_maximum; }
        }

        private string m_value;
        private float m_minimum;
        private float m_maximum;
    }
}
