namespace BeerRecipeCore
{
    public abstract class IngredientTypeBase
    {
        public IngredientTypeBase(string name, string notes)
        {
            Name = name;
            Notes = notes;
        }

        public string Name { get; }

        public string Notes { get; }
    }
}
