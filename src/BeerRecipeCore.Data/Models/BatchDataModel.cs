using System;
using System.Collections.ObjectModel;
using BeerRecipeCore;
using MvvmFoundation.Wpf;

namespace BeerRecipeCore.Data.Models
{
    public class BatchDataModel : ObservableObject, IBatch
    {
        public BatchDataModel(int batchId)
        {
            m_batchId = batchId;
        }

        public int BatchId
        {
            get { return m_batchId; }
        }

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

        public IGravityReading OriginalGravity
        {
            get { return m_originalGravity; }
            set
            {
                m_originalGravity = value;
                RaisePropertyChanged("OriginalGravity");
            }
        }

        public IGravityReading FinalGravity
        {
            get { return m_finalGravity; }
            set
            {
                m_finalGravity = value;
                RaisePropertyChanged("FinalGravity");
            }
        }

        public ObservableCollection<IGravityReading> RecordedGravityReadings
        {
            get { return m_recordedGravityReadings; }
            private set
            {
                m_recordedGravityReadings = value;
                RaisePropertyChanged("RecordedGravityReadings");
            }
        }

        int m_batchId;
        string m_brewerName;
        string m_assistantBrewerName;
        DateTime m_brewingDate;
        IRecipe m_recipe;
        IGravityReading m_originalGravity;
        IGravityReading m_finalGravity;
        ObservableCollection<IGravityReading> m_recordedGravityReadings = new ObservableCollection<IGravityReading>();
    }
}
