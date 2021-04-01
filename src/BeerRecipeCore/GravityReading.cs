using System;

namespace BeerRecipeCore
{
    public class GravityReading : IGravityReading
    {
        public GravityReading(double value, DateTime date)
        {
            Value = value;
            Date = date;
        }

        public double Value { get; set; }

        public DateTime Date { get; set; }
    }
}
