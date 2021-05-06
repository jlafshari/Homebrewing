using BeerRecipeCore.Yeast;

namespace HomebrewApi.Models
{
    public class YeastCharacteristics
    {
        public YeastType Type { get; set; }
        public Flocculation Flocculation { get; set; }
        public YeastForm Form { get; set; }
        public float MinTemperature { get; set; }
        public float MaxTemperature { get; set; }
        public float Attenuation { get; set; }
    }
}