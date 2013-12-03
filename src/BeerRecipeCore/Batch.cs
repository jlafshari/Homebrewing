using System;

namespace BeerRecipeCore
{
    public class Batch
    {
        public Batch(string brewerName, Recipe recipe)
        {
            m_brewerName = brewerName;
            m_recipe = recipe;
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

        public Recipe Recipe
        {
            get { return m_recipe; }
            set { m_recipe = value; }
        }

        public GravityReading OriginalGravity
        {
            get { return m_originalGravity; }
            set { m_originalGravity = value; }
        }

        public GravityReading FinalGravity
        {
            get { return m_finalGravity; }
            set { m_finalGravity = value; }
        }

        private string m_brewerName;
        private string m_assistantBrewerName;
        private DateTime m_brewingDate;
        private Recipe m_recipe;
        private GravityReading m_originalGravity;
        private GravityReading m_finalGravity;
    }
}
