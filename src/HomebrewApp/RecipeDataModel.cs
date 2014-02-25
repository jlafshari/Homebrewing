using System.Collections.ObjectModel;
using BeerRecipeCore;
using MvvmFoundation.Wpf;

namespace HomebrewApp
{
    public class RecipeDataModel : ObservableObject, IRecipe
    {
        public float Size
        {
            get { return m_size; }
            set
            {
                m_size = value;
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

        public Yeast Yeast
        {
            get { return m_yeast; }
            set
            {
                m_yeast = value;
                RaisePropertyChanged("Yeast");
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

        float m_size;
        int m_boilTime;
        Style m_style;
        ObservableCollection<IHopsIngredient> m_hopsIngredients = new ObservableCollection<IHopsIngredient>();
        ObservableCollection<IFermentableIngredient> m_fermentableIngredients = new ObservableCollection<IFermentableIngredient>();
        Yeast m_yeast;
        MashProfile m_mashProfile;
    }
}
