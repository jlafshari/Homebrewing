using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Formulas;
using BeerRecipeCore.Hops;
using BeerRecipeCore.Styles;
using BeerRecipeCore.Yeast;
using MvvmFoundation.Wpf;

namespace BeerRecipeCore.Data.Models
{
    public class RecipeDataModel : ObservableObject, IRecipe
    {
        public RecipeDataModel(int recipeId)
        {
            RecipeId = recipeId;
            m_fermentableIngredients.CollectionChanged += Ingredients_CollectionChanged;
            m_hopsIngredients.CollectionChanged += Ingredients_CollectionChanged;
        }

        public int RecipeId { get; }

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
                UpdateRecipeOutcome();
                RaisePropertyChanged("BoilTime");
            }
        }

        public Style Style
        {
            get { return m_style; }
            set
            {
                m_style = value;
                GetNewThresholdComparers();
                UpdateRecipeOutcome();
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
                UpdateRecipeOutcome();
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

        public float ExtractionEfficiency { get; set; }

        public StyleThresholdComparisonDataModel OriginalGravityStyleComparison
        {
            get { return m_originalGravityStyleComparison; }
            set
            {
                m_originalGravityStyleComparison = value;
                RaisePropertyChanged("OriginalGravityStyleComparison");
            }
        }

        public StyleThresholdComparisonDataModel FinalGravityStyleComparison
        {
            get { return m_finalGravityStyleComparison; }
            set
            {
                m_finalGravityStyleComparison = value;
                RaisePropertyChanged("FinalGravityStyleComparison");
            }
        }

        public StyleThresholdComparisonDataModel AbvStyleComparison
        {
            get { return m_abvStyleComparison; }
            set
            {
                m_abvStyleComparison = value;
                RaisePropertyChanged("AbvStyleComparison");
            }
        }

        public StyleThresholdComparisonDataModel BitternessStyleComparison
        {
            get { return m_bitternessStyleComparison; }
            set
            {
                m_bitternessStyleComparison = value;
                RaisePropertyChanged("BitternessStyleComparison");
            }
        }

        public StyleThresholdComparisonDataModel ColorStyleComparison
        {
            get { return m_colorStyleComparison; }
            set
            {
                m_colorStyleComparison = value;
                RaisePropertyChanged("ColorStyleComparison");
            }
        }

        public void UpdateRecipeOutcome()
        {
            if (m_size == 0)
                return;

            OriginalGravity = AlcoholUtility.GetOriginalGravity(m_fermentableIngredients, m_size, ExtractionEfficiency);
            if (OriginalGravityStyleComparison != null)
                OriginalGravityStyleComparison.Compare(m_originalGravity);

            if (m_yeastIngredient != null && m_yeastIngredient.YeastInfo != null)
            {
                FinalGravity = AlcoholUtility.GetFinalGravity(m_originalGravity, m_yeastIngredient.YeastInfo.Characteristics.Attenuation);
                if (FinalGravityStyleComparison != null)
                    FinalGravityStyleComparison.Compare(m_finalGravity);
            }

            if (m_finalGravity != 0)
            {
                AlcoholByVolume = AlcoholUtility.GetAlcoholByVolume(m_originalGravity, m_finalGravity);
                if (AbvStyleComparison != null)
                    AbvStyleComparison.Compare(m_alcoholByVolume);
            }

            AlcoholByWeight = AlcoholUtility.GetAlcoholByWeight(m_alcoholByVolume);
            Bitterness = BitternessUtility.GetBitterness(m_hopsIngredients, m_size, m_originalGravity);
            if (BitternessStyleComparison != null)
                BitternessStyleComparison.Compare(m_bitterness);
            Color = ColorUtility.GetColorInSrm(m_fermentableIngredients, m_size);
            if (ColorStyleComparison != null)
                ColorStyleComparison.Compare((float) m_color);
        }

        public void Ingredient_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateRecipeOutcome();
        }

        private void Ingredients_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateRecipeOutcome();
        }

        private void GetNewThresholdComparers()
        {
            if (m_style == null)
                return;

            ReadOnlyCollection<StyleThreshold> thresholds = m_style.Thresholds;
            OriginalGravityStyleComparison = new StyleThresholdComparisonDataModel(thresholds.Single(threshold => threshold.Value == "og"));
            FinalGravityStyleComparison = new StyleThresholdComparisonDataModel(thresholds.Single(threshold => threshold.Value == "fg"));
            AbvStyleComparison = new StyleThresholdComparisonDataModel(thresholds.Single(threshold => threshold.Value == "abv"));
            BitternessStyleComparison = new StyleThresholdComparisonDataModel(thresholds.Single(threshold => threshold.Value == "ibu"));
            ColorStyleComparison = new StyleThresholdComparisonDataModel(thresholds.Single(threshold => threshold.Value == "color"));
        }

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
        StyleThresholdComparisonDataModel m_originalGravityStyleComparison;
        StyleThresholdComparisonDataModel m_finalGravityStyleComparison;
        StyleThresholdComparisonDataModel m_abvStyleComparison;
        StyleThresholdComparisonDataModel m_bitternessStyleComparison;
        StyleThresholdComparisonDataModel m_colorStyleComparison;
    }
}
