using MvvmFoundation.Wpf;

namespace BeerRecipeCore.Data.Models
{
    public class SettingsDataModel : ObservableObject
    {
        public SettingsDataModel(int settingsId)
        {
            m_settingsId = settingsId;
        }

        public int SettingsId
        {
            get { return m_settingsId; }
        }

        public float RecipeSize
        {
            get { return m_recipeSize; }
            set
            {
                m_recipeSize = value;
                RaisePropertyChanged("RecipeSize");
            }
        }

        public int BoilTime
        {
            get { return m_boilTime; }
            set
            {
                m_boilTime = value;
                RaisePropertyChanged("BoilTime");
            }
        }

        public float ExtractionEfficiency
        {
            get { return m_extractionEfficiency; }
            set
            {
                m_extractionEfficiency = value;
                RaisePropertyChanged("ExtractionEfficiency");
            }
        }

        public float YeastWeight
        {
            get { return m_yeastWeight; }
            set
            {
                m_yeastWeight = value;
                RaisePropertyChanged("YeastWeight");
            }
        }

        int m_settingsId;
        float m_recipeSize;
        int m_boilTime;
        float m_extractionEfficiency;
        float m_yeastWeight;
    }
}
