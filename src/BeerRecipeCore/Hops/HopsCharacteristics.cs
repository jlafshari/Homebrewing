namespace BeerRecipeCore.Hops
{
    public class HopsCharacteristics
    {
        public HopsCharacteristics(float alphaAcid, float betaAcid)
        {
            AlphaAcid = alphaAcid;
            BetaAcid = betaAcid;
        }

        /// <summary>
        /// The alpha acid percentage.
        /// </summary>
        public float AlphaAcid { get; }

        /// <summary>
        /// The beta acid percentage.
        /// </summary>
        public float BetaAcid { get; }

        /// <summary>
        /// The Hop Stability Index: the percentage of hop alpha acid lost in 6 months of storage.
        /// </summary>
        public float Hsi { get; set; }
    }
}
