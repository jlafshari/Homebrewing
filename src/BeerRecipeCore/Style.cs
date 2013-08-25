using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerRecipeCore
{
    public class Style : BeerXmlObject
    {
        public Style(string name, int version, string notes, string category, int categoryNumber, string styleLetter, string styleGuide, string type,
            float ogMin, float ogMax, float fgMin, float fgMax, float ibuMin, float ibuMax, float colorMin, float colorMax, float carbMin, float carbMax,
            float abvMin, float abvMax, string profile, string ingredients, string examples)
            : base(name, version, notes)
        {
        }
    }
}
