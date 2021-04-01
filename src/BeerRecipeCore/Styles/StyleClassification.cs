namespace BeerRecipeCore.Styles
{
    public class StyleClassification
    {
        public StyleClassification(string styleLetter, string styleGuide)
        {
            StyleLetter = styleLetter;
            StyleGuide = styleGuide;
        }

        public string StyleLetter { get; }

        public string StyleGuide { get; }
    }
}
