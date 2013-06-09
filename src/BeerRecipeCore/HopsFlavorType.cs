using System;

namespace BeerRecipeCore
{
    [Flags]
    public enum HopsFlavorType
    {
        Bittering = 0x01,
        Aroma = 0x02
    }
}
