namespace BeerRecipeCore
{
    public class MashStep
    {
        public MashStep(MashType type)
        {
            Type = type;

            // decoction mash steps don't have any infusion water
            if (Type == MashType.Decoction)
                InfusionWaterTemperature = null;
        }

        public MashType Type { get; set; }

        /// <summary>
        /// The amount of water to add to the mash, in quarts.
        /// </summary>
        public float InfuseAmount { get; set; }

        /// <summary>
        /// The target temperature, in degrees Fahrenheit.
        /// </summary>
        public int TargetTemperature { get; set; }

        /// <summary>
        /// The duration of the mash step in minutes.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// The infusion water temperature, in degrees Fahrenheit.
        /// </summary>
        public int? InfusionWaterTemperature { get; set; }
    }
}
