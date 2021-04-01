using System;

namespace BeerRecipeCore.Hops
{
    [Flags]
    public enum HopsFlavorType
    {
        Bittering = 0x01,
        Aroma = 0x02
    }
}
