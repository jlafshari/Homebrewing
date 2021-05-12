using System;

namespace BeerRecipeCore.Hops
{
    [Flags]
    public enum HopFlavorType
    {
        Bittering = 0x01,
        Aroma = 0x02
    }
}
