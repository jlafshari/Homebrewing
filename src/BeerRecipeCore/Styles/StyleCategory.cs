using Utility;

namespace BeerRecipeCore.Styles
{
    public class StyleCategory
    {
        public StyleCategory(string name, int number, string type)
        {
            Name = name;
            Number = number;
            Type = EnumConverter.Parse<StyleType>(type);
        }

        public string Name { get; }

        public int Number { get; }

        public StyleType Type { get; }
    }
}
