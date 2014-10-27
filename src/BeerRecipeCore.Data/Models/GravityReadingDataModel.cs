using System;
using MvvmFoundation.Wpf;

namespace BeerRecipeCore.Data.Models
{
    public class GravityReadingDataModel : ObservableObject, IGravityReading
    {
        public GravityReadingDataModel(int gravityReadingId)
        {
            m_gravityReadingId = gravityReadingId;
        }

        public int GravityReadingId
        {
            get { return m_gravityReadingId; }
        }

        public double Value
        {
            get { return m_value; }
            set
            {
                m_value = value;
                RaisePropertyChanged("Value");
            }
        }

        public DateTime Date
        {
            get { return m_date; }
            set
            {
                m_date = value;
                RaisePropertyChanged("Date");
            }
        }

        int m_gravityReadingId;
        double m_value;
        DateTime m_date;
    }
}
