using BeerRecipeCore.Fermentables;

namespace HomebrewApi.Models
{
    public record FermentableCharacteristics
    (
        float? Yield,
        float? YieldByWeight,
        double Color,
        float? DiastaticPower,
        FermentableType Type,
        MaltCategory? MaltCategory,
        int GravityPoint
    );
}