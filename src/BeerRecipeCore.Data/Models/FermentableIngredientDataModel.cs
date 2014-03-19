using BeerRecipeCore;
using MvvmFoundation.Wpf;

namespace BeerRecipeCore.Data.Models
{
    public class FermentableIngredientDataModel : ObservableObject, IFermentableIngredient
    {
        public FermentableIngredientDataModel(Fermentable fermentableInfo, int fermentableId)
        {
            m_fermentableInfo = fermentableInfo;
            m_fermentableId = fermentableId;
        }

        public int FermentableId
        {
            get { return m_fermentableId; }
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

        int m_fermentableId;
        float m_amount;
        Fermentable m_fermentableInfo;
    }
}
