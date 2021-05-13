namespace BeerRecipeCore.Hops
{
    internal class HopIngredient : IHopIngredient
    {
        public float Amount { get; set; }
        public int Time { get; set; }
        public int? DryHopTime { get; set; }
        public HopFlavorType FlavorType { get; set; }
        public HopForm Form { get; set; }
        public HopUse Use { get; set; }
        public Hop HopInfo { get; set; }
    }
}