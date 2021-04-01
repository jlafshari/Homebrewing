using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using BeerRecipeCore;
using BeerRecipeCore.Formulas;
using MvvmFoundation.Wpf;

namespace BeerRecipeCore.Data.Models
{
    public class BatchDataModel : ObservableObject, IBatch
    {
        public BatchDataModel(int batchId)
        {
            BatchId = batchId;
            m_recordedGravityReadings.CollectionChanged += Ingredients_CollectionChanged;
        }

        public int BatchId { get; }

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

        public float AlcoholByVolume
        {
            get { return m_alcoholByVolume; }
            private set
            {
                m_alcoholByVolume = value;
                RaisePropertyChanged("AlcoholByVolume");
            }
        }

        public float AlcoholByWeight
        {
            get { return m_alcoholByWeight; }
            private set
            {
                m_alcoholByWeight = value;
                RaisePropertyChanged("AlcoholByWeight");
            }
        }

        public void Ingredient_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateBatchOutcome();
        }

        private void Ingredients_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateBatchOutcome();
        }

        public void UpdateBatchOutcome()
        {
            if (m_recordedGravityReadings.Count > 1)
            {
                AlcoholByVolume = AlcoholUtility.GetAlcoholByVolume((float) m_recordedGravityReadings.First().Value, (float) m_recordedGravityReadings.Last().Value);
                AlcoholByWeight = AlcoholUtility.GetAlcoholByWeight(m_alcoholByVolume);
            }
        }

        string m_brewerName;
        string m_assistantBrewerName;
        DateTime m_brewingDate;
        IRecipe m_recipe;
        IGravityReading m_originalGravity;
        IGravityReading m_finalGravity;
        ObservableCollection<IGravityReading> m_recordedGravityReadings = new ObservableCollection<IGravityReading>();
        float m_alcoholByVolume;
        float m_alcoholByWeight;
    }
}
