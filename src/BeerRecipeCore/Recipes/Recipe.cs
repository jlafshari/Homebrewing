using System.Collections.Generic;
using System.Collections.ObjectModel;
using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Hops;
using BeerRecipeCore.Mash;
using BeerRecipeCore.Styles;
using BeerRecipeCore.Yeast;

namespace BeerRecipeCore.Recipes
{
    internal class Recipe : IRecipe
    {
        public float Size { get; set; }
        public int BoilTime { get; set; }
        public IStyle Style { get; set; }
        public ObservableCollection<IHopIngredient> HopIngredients { get; }
        public List<IFermentableIngredient> FermentableIngredients { get; init; }
        public IYeastIngredient YeastIngredient { get; set; }
        public MashProfile MashProfile { get; set; }
        public string Name { get; set; }
    }
}