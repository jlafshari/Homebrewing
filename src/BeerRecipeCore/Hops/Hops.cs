namespace BeerRecipeCore.Hops
{
    public class Hops : IngredientTypeBase
    {
        public Hops(string name, HopsCharacteristics characteristics, string notes, string origin)
            : base(name, notes)
        {
            Characteristics = characteristics;
            Origin = origin;
        }

        public HopsCharacteristics Characteristics { get; }

        public string Origin { get; } = "";
    }
}
