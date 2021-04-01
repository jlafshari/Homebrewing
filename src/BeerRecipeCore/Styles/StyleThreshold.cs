namespace BeerRecipeCore.Styles
{
    public class StyleThreshold
    {
        public StyleThreshold(string value, float minimum, float maximum)
        {
            Value = value;
            Minimum = minimum;
            Maximum = maximum;
        }

        public string Value { get; }

        public float Minimum { get; }

        public float Maximum { get; }
    }
}
