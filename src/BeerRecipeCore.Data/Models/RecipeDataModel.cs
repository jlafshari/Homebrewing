using System.Collections.ObjectModel;
using BeerRecipeCore.Formulas;
using MvvmFoundation.Wpf;

namespace BeerRecipeCore.Data.Models
{
    public class RecipeDataModel : ObservableObject, IRecipe
    {
        public RecipeDataModel(int recipeId)
        {
            m_recipeId = recipeId;
        }

        public int RecipeId
        {
            get { return m_recipeId; }
        }

        public float Size
        {
            get { return m_size; }
            set
            {
                m_size = value;
                UpdateRecipeOutcome();
                RaisePropertyChanged("Size");
            }
        }

        public int BoilTime
        {
            get { return m_boilTime; }
            set
            {
                m_boilTime = value;
                RaisePropertyChanged("BoilTime");
            }
        }

        public Style Style
        {
            get { return m_style; }
            set
            {
                m_style = value;
                RaisePropertyChanged("Style");
            }
        }

        public ObservableCollection<IHopsIngredient> HopsIngredients
        {
            get { return m_hopsIngredients; }
            private set
            {
                m_hopsIngredients = value;
                RaisePropertyChanged("HopsIngredients");
            }
        }

        public ObservableCollection<IFermentableIngredient> FermentableIngredients
        {
            get { return m_fermentableIngredients; }
            private set
            {
                m_fermentableIngredients = value;
                RaisePropertyChanged("FermentableIngredients");
            }
        }

        public IYeastIngredient YeastIngredient
        {
            get { return m_yeastIngredient; }
            set
            {
                m_yeastIngredient = value;
                RaisePropertyChanged("YeastIngredient");
            }
        }

        public MashProfile MashProfile
        {
            get { return m_mashProfile; }
            set
            {
                m_mashProfile = value;
                RaisePropertyChanged("MashProfile");
            }
        }

        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                RaisePropertyChanged("Name");
            }
        }

        public float OriginalGravity
        {
            get { return m_originalGravity; }
            private set
            {
                m_originalGravity = value;
                RaisePropertyChanged("OriginalGravity");
            }
        }

        public float FinalGravity
        {
            get { return m_finalGravity; }
            private set
            {
                m_finalGravity = value;
                RaisePropertyChanged("FinalGravity");
            }
        }

        public float AlcoholByVolume
        {
            get { return m_alcoholByVolume; }
            private set
            {
                m_alcoholByVolume = value;
                RaisePropertyChanged("AlcoholByVolume");
            }
        }

        public float AlcoholByWeight
        {
            get { return m_alcoholByWeight; }
            private set
            {
                m_alcoholByWeight = value;
                RaisePropertyChanged("AlcoholByWeight");
            }
        }

        public int Bitterness
        {
            get { return m_bitterness; }
            private set
            {
                m_bitterness = value;
                RaisePropertyChanged("Bitterness");
            }
        }

        public double Color
        {
            get { return m_color; }
            set
            {
                m_color = value;
                RaisePropertyChanged("Color");
            }
        }

        public void UpdateRecipeOutcome()
        {
            if (m_size == 0)
                return;

            OriginalGravity = AlcoholUtility.GetOriginalGravity(m_fermentableIngredients, m_size);

            if (m_yeastIngredient != null && m_yeastIngredient.YeastInfo != null)
                FinalGravity = AlcoholUtility.GetFinalGravity(m_originalGravity, m_yeastIngredient.YeastInfo.Characteristics.Attenuation);

            if (m_finalGravity != 0)
                AlcoholByVolume = AlcoholUtility.GetAlcoholByVolume(m_originalGravity, m_finalGravity);

            AlcoholByWeight = AlcoholUtility.GetAlcoholByWeight(m_alcoholByVolume);
            Bitterness = BitternessUtility.GetBitterness(m_hopsIngredients, m_size, m_originalGravity);
            Color = ColorUtility.GetColorInSrm(m_fermentableIngredients, m_size);
        }

        int m_recipeId;
        float m_size;
        int m_boilTime;
        Style m_style;
        ObservableCollection<IHopsIngredient> m_hopsIngredients = new ObservableCollection<IHopsIngredient>();
        ObservableCollection<IFermentableIngredient> m_fermentableIngredients = new ObservableCollection<IFermentableIngredient>();
        IYeastIngredient m_yeastIngredient;
        MashProfile m_mashProfile;
        string m_name;
        float m_originalGravity;
        float m_finalGravity;
        float m_alcoholByVolume;
        float m_alcoholByWeight;
        int m_bitterness;
        double m_color;
    }
}
