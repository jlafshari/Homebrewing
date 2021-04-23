using System.Collections.Generic;
using BeerRecipeCore.Styles;

namespace HomebrewApi.Models.Dtos
{
    public record StyleDto(string Id, string Name, List<StyleThreshold> Thresholds);
}