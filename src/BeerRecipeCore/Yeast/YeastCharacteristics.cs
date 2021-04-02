using Utility;

namespace BeerRecipeCore.Yeast
{
    public class YeastCharacteristics
    {
        public YeastCharacteristics(string type, string flocculation, string form)
        {
            Type = EnumConverter.Parse<YeastType>(type);
            Flocculation = EnumConverter.Parse<Flocculation>(flocculation);
            Form = EnumConverter.Parse<YeastForm>(form);
        }

        public YeastType Type { get; }

        public Flocculation Flocculation { get; }

        public YeastForm Form { get; }

        /// <summary>
        /// The minimum fermenting temperature, in degrees Celsius.
        /// </summary>
        public float MinTemperature { get; init; }

        /// <summary>
        /// The maximum fermenting temperature, in degrees Celsius.
        /// </summary>
        public float MaxTemperature { get; init; }

        /// <summary>
        /// The average attenuation percentage.
        /// </summary>
        public float Attenuation { get; init; }
    }
}
