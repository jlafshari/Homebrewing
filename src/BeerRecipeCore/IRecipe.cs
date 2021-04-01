using System.Collections.ObjectModel;
using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Hops;
using BeerRecipeCore.Styles;
using BeerRecipeCore.Yeast;

namespace BeerRecipeCore
{
    public interface IRecipe
    {
        /// <summary>
        /// The final amount of beer in gallons.
        /// </summary>
        float Size { get; set; }

        /// <summary>
        /// The boil time in minutes.
        /// </summary>
        int BoilTime { get; set; }
        Style Style { get; set; }
        ObservableCollection<IHopsIngredient> HopsIngredients { get; }
        ObservableCollection<IFermentableIngredient> FermentableIngredients { get; }
        IYeastIngredient YeastIngredient { get; set; }
        MashProfile MashProfile { get; set; }
        string Name { get; set; }
    }
}
