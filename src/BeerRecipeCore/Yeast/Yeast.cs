namespace BeerRecipeCore.Yeast
{
    public class Yeast : IngredientTypeBase
    {
        public Yeast(string name, YeastCharacteristics characteristics, string notes, string laboratory, string productId)
            : base(name, notes)
        {
            Characteristics = characteristics;
            Laboratory = laboratory;
            ProductId = productId;
        }

        public YeastCharacteristics Characteristics { get; }

        /// <summary>
        /// The name of the laboratory that produced the yeast.
        /// </summary>
        public string Laboratory { get; }

        /// <summary>
        /// The manufacturer’s product ID label or number that identifies this particular strain of yeast.
        /// </summary>
        public string ProductId { get; }
    }
}
