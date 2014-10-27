using System;
using System.Collections.ObjectModel;

namespace BeerRecipeCore
{
    public class Batch : IBatch
    {
        public Batch(string brewerName, IRecipe recipe)
        {
            m_brewerName = brewerName;
            m_recipe = recipe;
            m_assistantBrewerName = null;
            m_originalGravity = null;
            m_finalGravity = null;
            m_recordedGravityReadings = new ObservableCollection<IGravityReading>();
        }

        public string BrewerName
        {
            get { return m_brewerName; }
            set { m_brewerName = value; }
        }

        public string AssistantBrewerName
        {
            get { return m_assistantBrewerName; }
            set { m_assistantBrewerName = value; }
        }

        public DateTime BrewingDate
        {
            get { return m_brewingDate; }
            set { m_brewingDate = value; }
        }

        public IRecipe Recipe
        {
            get { return m_recipe; }
            set { m_recipe = value; }
        }

        public IGravityReading OriginalGravity
        {
            get { return m_originalGravity; }
            set { m_originalGravity = value; }
        }

        public IGravityReading FinalGravity
        {
            get { return m_finalGravity; }
            set { m_finalGravity = value; }
        }

        public ObservableCollection<IGravityReading> RecordedGravityReadings
        {
            get { return m_recordedGravityReadings; }
            private set { m_recordedGravityReadings = value; }
        }

        private string m_brewerName;
        private string m_assistantBrewerName;
        private DateTime m_brewingDate;
        private IRecipe m_recipe;
        private IGravityReading m_originalGravity;
        private IGravityReading m_finalGravity;
        private ObservableCollection<IGravityReading> m_recordedGravityReadings;
    }
}
