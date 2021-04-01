namespace BeerRecipeCore.Yeast
{
    public class YeastIngredient : IYeastIngredient
    {
        public YeastIngredient(Yeast yeastInfo)
        {
            YeastInfo = yeastInfo;
        }

        public float Weight { get; set; }

        public float Volume { get; set; }

        public Yeast YeastInfo { get; set; }
    }
}
