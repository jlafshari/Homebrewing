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
        GravityReading OriginalGravity { get; set; }
        GravityReading FinalGravity { get; set; }
        ObservableCollection<GravityReading> RecordedGravityReadings { get; }
    }
}
