namespace BeerRecipeCore.Formulas
{
    public static class AlcoholUtility
    {
        public static float GetAlcoholByVolume(float startingGravity, float finalGravity)
        {
            return (1.05f * (startingGravity - finalGravity)) / (finalGravity * 0.79f) * 100;
        }

        public static float GetAlcoholByWeight(float alcoholByVolume)
        {
            return alcoholByVolume * 0.79336f;
        }
    }
}
