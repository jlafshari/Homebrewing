using System.Xml.Linq;

namespace BeerRecipeCore.BeerXml
{
    public static class BeerXmlExtensions
    {
        public static string GetChildElementValue(this XElement element, string childElementName)
        {
            return element.Element(childElementName)?.Value;
        }
    }
}