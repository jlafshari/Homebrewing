using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace BeerRecipeCore.Styles
{
    public class StyleCategory
    {
        public StyleCategory(string name, int number, string type)
        {
            m_name = name;
            m_number = number;
            m_type = EnumConverter.Parse<StyleType>(type);
        }

        public string Name
        {
            get { return m_name; }
        }

        public int Number
        {
            get { return m_number; }
        }

        public StyleType Type
        {
            get { return m_type; }
        }

        private string m_name;
        private int m_number;
        private StyleType m_type;
    }
}
