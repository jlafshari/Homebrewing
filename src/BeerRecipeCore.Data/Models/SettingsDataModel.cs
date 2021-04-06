using MvvmFoundation.Wpf;

namespace BeerRecipeCore.Data.Models
{
    public class SettingsDataModel : ObservableObject
    {
        public SettingsDataModel(int settingsId)
        {
            SettingsId = settingsId;
        }

        public int SettingsId { get; }

        public float RecipeSize
        {
            get { return m_recipeSize; }
            set
            {
                m_recipeSize = value;
                RaisePropertyChanged(nameof(RecipeSize));
            }
        }

        public int BoilTime
        {
            get { return m_boilTime; }
            set
            {
                m_boilTime = value;
                RaisePropertyChanged(nameof(BoilTime));
            }
        }

        public float ExtractionEfficiency
        {
            get { return m_extractionEfficiency; }
            set
            {
                m_extractionEfficiency = value;
                RaisePropertyChanged(nameof(ExtractionEfficiency));
            }
        }

        public float YeastWeight
        {
            get { return m_yeastWeight; }
            set
            {
                m_yeastWeight = value;
                RaisePropertyChanged(nameof(YeastWeight));
            }
        }

        public float HopsAmount
        {
            get { return m_hopsAmount; }
            set
            {
                m_hopsAmount = value;
                RaisePropertyChanged(nameof(HopsAmount));
            }
        }

        float m_recipeSize;
        int m_boilTime;
        float m_extractionEfficiency;
        float m_yeastWeight;
        float m_hopsAmount;
    }
}
