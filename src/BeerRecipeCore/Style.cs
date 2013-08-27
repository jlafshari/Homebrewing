using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerRecipeCore
{
    public class Style : BeerXmlObject
    {
        public Style(string name, StyleCategory category, string notes, string styleLetter, string styleGuide, string profile, string ingredients, string examples)
            : base(name, notes)
        {
        }
    }
}
