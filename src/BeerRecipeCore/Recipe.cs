using System.Collections.ObjectModel;

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

        public Collection<IHopsIngredient> HopsIngredients
        {
            get { return m_hopsIngredients; }
            private set { m_hopsIngredients = value; }
        }

        public Collection<IFermentableIngredient> FermentableIngredients
        {
            get { return m_fermentableIngredients; }
            private set { m_fermentableIngredients = value; }
        }

        public Yeast Yeast
        {
            get { return m_yeast; }
            set { m_yeast = value; }
        }

        public MashProfile MashProfile
        {
            get { return m_mashProfile; }
            set { m_mashProfile = value; }
        }

        float m_size;
        int m_boilTime;
        Style m_style;
        Collection<IHopsIngredient> m_hopsIngredients = new Collection<IHopsIngredient>();
        Collection<IFermentableIngredient> m_fermentableIngredients = new Collection<IFermentableIngredient>();
        Yeast m_yeast;
        MashProfile m_mashProfile;
    }
}
