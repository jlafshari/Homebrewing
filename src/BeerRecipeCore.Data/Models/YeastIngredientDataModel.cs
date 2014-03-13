using BeerRecipeCore;
using MvvmFoundation.Wpf;

namespace BeerRecipeCore.Data.Models
{
    public class YeastIngredientDataModel : ObservableObject, IYeastIngredient
    {
        public YeastIngredientDataModel(Yeast yeastInfo)
        {
            m_yeastInfo = yeastInfo;
        }

        public float Weight
        {
            get
            {
                return m_weight;
            }
            set
            {
                m_weight = value;
                RaisePropertyChanged("Weight");
            }
        }

        public float Volume
        {
            get
            {
                return m_volume;
            }
            set
            {
                m_volume = value;
                RaisePropertyChanged("Volume");
            }
        }

        public Yeast YeastInfo
        {
            get { return m_yeastInfo; }
        }

        private float m_weight;
        private float m_volume;
        private Yeast m_yeastInfo;
    }
}
