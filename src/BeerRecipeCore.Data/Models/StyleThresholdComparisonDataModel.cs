using System;
using BeerRecipeCore.Styles;
using MvvmFoundation.Wpf;

namespace BeerRecipeCore.Data.Models
{
    public sealed class StyleThresholdComparisonDataModel : ObservableObject
    {
        public StyleThresholdComparisonDataModel(StyleThreshold styleThreshold)
        {
            StyleThreshold = styleThreshold;
        }

        public StyleThreshold StyleThreshold { get; }

        public StyleThresholdStatus Status
        {
            get { return m_status; }
            private set
            {
                m_status = value;
                RaisePropertyChanged(nameof(Status));
            }
        }

        public float Difference
        {
            get { return m_difference; }
            private set
            {
                m_difference = value;
                RaisePropertyChanged(nameof(Difference));
            }
        }

        public void Compare(float recipeValue)
        {
            const int decimalPlaces = 3;
            if (recipeValue > StyleThreshold.Maximum)
            {
                Status = StyleThresholdStatus.Above;
                Difference = (float) Math.Round(recipeValue - StyleThreshold.Maximum, decimalPlaces);
            }
            else if (recipeValue < StyleThreshold.Minimum)
            {
                Status = StyleThresholdStatus.Below;
                Difference = (float) Math.Round(StyleThreshold.Minimum - recipeValue, decimalPlaces);
            }
            else
            {
                Status = StyleThresholdStatus.WithinRange;
                Difference = 0;
            }
        }

        StyleThresholdStatus m_status;
        float m_difference;
    }
}
