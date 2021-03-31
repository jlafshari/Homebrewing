using System;
using BeerRecipeCore.Styles;
using MvvmFoundation.Wpf;

namespace BeerRecipeCore.Data.Models
{
    public sealed class StyleThresholdComparisonDataModel : ObservableObject
    {
        public StyleThresholdComparisonDataModel(StyleThreshold styleThreshold)
        {
            m_styleThreshold = styleThreshold;
        }

        public StyleThreshold StyleThreshold
        {
            get { return m_styleThreshold; }
        }

        public StyleThresholdStatus Status
        {
            get { return m_status; }
            private set
            {
                m_status = value;
                RaisePropertyChanged("Status");
            }
        }

        public float Difference
        {
            get { return m_difference; }
            private set
            {
                m_difference = value;
                RaisePropertyChanged("Difference");
            }
        }

        public void Compare(float recipeValue)
        {
            const int decimalPlaces = 3;
            if (recipeValue > m_styleThreshold.Maximum)
            {
                Status = StyleThresholdStatus.Above;
                Difference = (float) Math.Round(recipeValue - m_styleThreshold.Maximum, decimalPlaces);
            }
            else if (recipeValue < m_styleThreshold.Minimum)
            {
                Status = StyleThresholdStatus.Below;
                Difference = (float) Math.Round(m_styleThreshold.Minimum - recipeValue, decimalPlaces);
            }
            else
            {
                Status = StyleThresholdStatus.WithinRange;
                Difference = 0;
            }
        }

        StyleThreshold m_styleThreshold;
        StyleThresholdStatus m_status;
        float m_difference;
    }
}
