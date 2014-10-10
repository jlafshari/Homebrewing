using System;

namespace BeerRecipeCore
{
    public interface IGravityReading
    {
        double Value { get; set; }
        DateTime Date { get; set; }
    }
}
