using System;
using MvvmFoundation.Wpf;

namespace BeerRecipeCore.Data.Models
{
    public class GravityReadingDataModel : ObservableObject, IGravityReading
    {
        public GravityReadingDataModel(int gravityReadingId)
        {
            GravityReadingId = gravityReadingId;
        }

        public int GravityReadingId { get; }

        public double Value
        {
            get { return m_value; }
            set
            {
                m_value = value;
                RaisePropertyChanged(nameof(Value));
            }
        }

        public DateTime Date
        {
            get { return m_date; }
            set
            {
                m_date = value;
                RaisePropertyChanged(nameof(Date));
            }
        }

        double m_value;
        DateTime m_date;
    }
}
