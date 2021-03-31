using System.Collections.ObjectModel;
using BeerRecipeCore.Styles;

namespace BeerRecipeCore
{
    public class Recipe : IRecipe
    {
        /// <summary>
        /// The final amount of beer in gallons.
        /// </summary>
        public float Size
        {
            get { return m_size; }
            set { m_size = value; }
        }

        /// <summary>
        /// The boil time in minutes.
        /// </summary>
        public int BoilTime
        {
            get { return m_boilTime; }
            set { m_boilTime = value; }
        }

        public Style Style
        {
            get { return m_style; }
            set { m_style = value; }
        }

        public ObservableCollection<IHopsIngredient> HopsIngredients
        {
            get { return m_hopsIngredients; }
            private set { m_hopsIngredients = value; }
        }

        public ObservableCollection<IFermentableIngredient> FermentableIngredients
        {
            get { return m_fermentableIngredients; }
            private set { m_fermentableIngredients = value; }
        }

        public IYeastIngredient YeastIngredient
        {
            get { return m_yeastIngredient; }
            set { m_yeastIngredient = value; }
        }

        public MashProfile MashProfile
        {
            get { return m_mashProfile; }
            set { m_mashProfile = value; }
        }

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        float m_size;
        int m_boilTime;
        Style m_style;
        ObservableCollection<IHopsIngredient> m_hopsIngredients = new ObservableCollection<IHopsIngredient>();
        ObservableCollection<IFermentableIngredient> m_fermentableIngredients = new ObservableCollection<IFermentableIngredient>();
        IYeastIngredient m_yeastIngredient;
        MashProfile m_mashProfile;
        string m_name;
    }
}
