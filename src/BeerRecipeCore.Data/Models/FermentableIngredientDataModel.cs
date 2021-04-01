using BeerRecipeCore.Fermentables;
using MvvmFoundation.Wpf;

namespace BeerRecipeCore.Data.Models
{
    public class FermentableIngredientDataModel : ObservableObject, IFermentableIngredient
    {
        public FermentableIngredientDataModel(Fermentable fermentableInfo, int fermentableId)
        {
            FermentableInfo = fermentableInfo;
            FermentableId = fermentableId;
        }

        public int FermentableId { get; }

        public float Amount
        {
            get { return m_amount; }
            set
            {
                m_amount = value;
                RaisePropertyChanged("Amount");
            }
        }

        public Fermentable FermentableInfo { get; }

        float m_amount;
    }
}
