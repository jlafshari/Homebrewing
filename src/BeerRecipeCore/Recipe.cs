using System.Collections.ObjectModel;
using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Hops;
using BeerRecipeCore.Styles;
using BeerRecipeCore.Yeast;

namespace BeerRecipeCore
{
    public class Recipe : IRecipe
    {
        /// <summary>
        /// The final amount of beer in gallons.
        /// </summary>
        public float Size { get; set; }

        /// <summary>
        /// The boil time in minutes.
        /// </summary>
        public int BoilTime { get; set; }

        public Style Style { get; set; }

        public ObservableCollection<IHopsIngredient> HopsIngredients { get; private set; } = new ObservableCollection<IHopsIngredient>();

        public ObservableCollection<IFermentableIngredient> FermentableIngredients { get; private set; } = new ObservableCollection<IFermentableIngredient>();

        public IYeastIngredient YeastIngredient { get; set; }

        public MashProfile MashProfile { get; set; }

        public string Name { get; set; }
    }
}
