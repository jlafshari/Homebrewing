namespace BeerRecipeCore.Fermentables
{
    public class Fermentable : IngredientTypeBase
    {
        public Fermentable(string name, FermentableCharacteristics characteristics, string notes, string origin)
            : base(name, notes)
        {
            Characteristics = characteristics;
            Origin = origin;
        }

        public FermentableCharacteristics Characteristics { get; }

        public string Origin { get; } = "";
    }
}
