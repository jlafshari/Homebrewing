using System.ComponentModel;
using BeerRecipeCore.Data;
using BeerRecipeCore.Data.Models;
using MvvmFoundation.Wpf;

namespace HomebrewApp
{
    public sealed class SettingsViewModel : ObservableObject
    {
        public SettingsViewModel()
        {
            m_settings = SettingsUtility.GetSavedSettings();
            m_settings.PropertyChanged += Setting_PropertyChanged;
        }

        public SettingsDataModel Settings
        {
            get { return m_settings; }
        }

        public void Setting_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SettingsUtility.UpdateSettings(m_settings);
        }

        readonly SettingsDataModel m_settings;
    }
}
