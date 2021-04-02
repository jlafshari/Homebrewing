using System;
using System.Collections.ObjectModel;

namespace BeerRecipeCore
{
    public class Batch : IBatch
    {
        public Batch(string brewerName, IRecipe recipe)
        {
            BrewerName = brewerName;
            Recipe = recipe;
            AssistantBrewerName = null;
            OriginalGravity = null;
            FinalGravity = null;
        }

        public string BrewerName { get; set; }

        public string AssistantBrewerName { get; set; }

        public DateTime BrewingDate { get; set; }

        public IRecipe Recipe { get; set; }

        public IGravityReading OriginalGravity { get; set; }

        public IGravityReading FinalGravity { get; set; }

        public ObservableCollection<IGravityReading> RecordedGravityReadings { get; } = new ObservableCollection<IGravityReading>();
    }
}
