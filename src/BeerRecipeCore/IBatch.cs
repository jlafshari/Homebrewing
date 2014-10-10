using System;
using System.Collections.ObjectModel;

namespace BeerRecipeCore
{
    public interface IBatch
    {
        string BrewerName { get; set; }
        string AssistantBrewerName { get; set; }
        DateTime BrewingDate { get; set; }
        IRecipe Recipe { get; set; }
        IGravityReading OriginalGravity { get; set; }
        IGravityReading FinalGravity { get; set; }
        ObservableCollection<IGravityReading> RecordedGravityReadings { get; }
    }
}
