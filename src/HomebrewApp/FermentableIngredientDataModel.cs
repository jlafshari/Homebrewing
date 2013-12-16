using BeerRecipeCore;
using MvvmFoundation.Wpf;

namespace HomebrewApp
{
    public class FermentableIngredientDataModel : ObservableObject, IFermentableIngredient
    {
        public FermentableIngredientDataModel(Fermentable fermentableInfo)
        {
            m_fermentableInfo = fermentableInfo;
        }

        public float Amount
        {
            get { return m_amount; }
            set
            {
                m_amount = value;
                RaisePropertyChanged("Amount");
            }
        }

        public Fermentable FermentableInfo
        {
            get { return m_fermentableInfo; }
        }

        private float m_amount;
        private Fermentable m_fermentableInfo;
    }
}
