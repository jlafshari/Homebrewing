namespace BeerRecipeCore.Yeast
{
    public record YeastCharacteristics(YeastType Type, Flocculation Flocculation, YeastForm Form,
        float MinTemperature, float MaxTemperature, float Attenuation);
}
