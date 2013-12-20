namespace BeerRecipeCore
{
    public class Yeast : IngredientTypeBase
    {
        public Yeast(string name, YeastCharacteristics characteristics, string notes, string laboratory, string productId)
            : base(name, notes)
        {
            m_characteristics = characteristics;
            m_laboratory = laboratory;
            m_productId = productId;
        }

        public YeastCharacteristics Characteristics
        {
            get { return m_characteristics; }
        }

        /// <summary>
        /// The name of the laboratory that produced the yeast.
        /// </summary>
        public string Laboratory
        {
            get { return m_laboratory; }
        }

        /// <summary>
        /// The manufacturer’s product ID label or number that identifies this particular strain of yeast.
        /// </summary>
        public string ProductId
        {
            get { return m_productId; }
        }

        private string m_laboratory;
        private string m_productId;
        private YeastCharacteristics m_characteristics;
    }
}
