namespace BeerRecipeCore
{
    public class StyleClassification
    {
        public StyleClassification(string styleLetter, string styleGuide)
        {
            m_styleLetter = styleLetter;
            m_styleGuide = styleGuide;
        }

        public string StyleLetter
        {
            get { return m_styleLetter; }
        }

        public string StyleGuide
        {
            get { return m_styleGuide; }
        }

        private string m_styleLetter;
        private string m_styleGuide;
    }
}
