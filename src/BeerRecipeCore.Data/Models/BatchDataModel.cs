using System;
using System.Collections.ObjectModel;
using BeerRecipeCore;
using MvvmFoundation.Wpf;

namespace BeerRecipeCore.Data.Models
{
    public class BatchDataModel : ObservableObject, IBatch
    {
        public string BrewerName
        {
            get { return m_brewerName; }
            set
            {
                m_brewerName = value;
                RaisePropertyChanged("BrewerName");
            }
        }

        public string AssistantBrewerName
        {
            get { return m_assistantBrewerName; }
            set
            {
                m_assistantBrewerName = value;
                RaisePropertyChanged("AssistantBrewerName");
            }
        }

        public DateTime BrewingDate
        {
            get { return m_brewingDate; }
            set
            {
                m_brewingDate = value;
                RaisePropertyChanged("BrewingDate");
            }
        }

        public IRecipe Recipe
        {
            get { return m_recipe; }
            set
            {
                m_recipe = value;
                RaisePropertyChanged("Recipe");
            }
        }

        public GravityReading OriginalGravity
        {
            get { return m_originalGravity; }
            set
            {
                m_originalGravity = value;
                RaisePropertyChanged("OriginalGravity");
            }
        }

        public GravityReading FinalGravity
        {
            get { return m_finalGravity; }
            set
            {
                m_finalGravity = value;
                RaisePropertyChanged("FinalGravity");
            }
        }

        public ObservableCollection<GravityReading> RecordedGravityReadings
        {
            get { return m_recordedGravityReadings; }
            private set
            {
                m_recordedGravityReadings = value;
                RaisePropertyChanged("RecordedGravityReadings");
            }
        }

        string m_brewerName;
        string m_assistantBrewerName;
        DateTime m_brewingDate;
        IRecipe m_recipe;
        GravityReading m_originalGravity;
        GravityReading m_finalGravity;
        ObservableCollection<GravityReading> m_recordedGravityReadings = new ObservableCollection<GravityReading>();
    }
}
