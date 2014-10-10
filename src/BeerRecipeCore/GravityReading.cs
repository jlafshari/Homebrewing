using System;

namespace BeerRecipeCore
{
    public class GravityReading : IGravityReading
    {
        public GravityReading(double value, DateTime date)
        {
            m_value = value;
            m_date = date;
        }

        public double Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public DateTime Date
        {
            get { return m_date; }
            set { m_date = value; }
        }

        private double m_value;
        private DateTime m_date;
    }
}
