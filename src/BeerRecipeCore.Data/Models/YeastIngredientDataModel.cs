using BeerRecipeCore.Yeast;
using MvvmFoundation.Wpf;

namespace BeerRecipeCore.Data.Models
{
    public class YeastIngredientDataModel : ObservableObject, IYeastIngredient
    {
        public YeastIngredientDataModel(Yeast.Yeast yeastInfo, int yeastIngredientId)
        {
            m_yeastInfo = yeastInfo;
            m_yeastIngredientId = yeastIngredientId;
        }

        public int YeastIngredientId
        {
            get { return m_yeastIngredientId; }
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

        public Yeast.Yeast YeastInfo
        {
            get
            {
                return m_yeastInfo;
            }
            set
            {
                m_yeastInfo = value;
                RaisePropertyChanged("YeastInfo");
            }
        }

        int m_yeastIngredientId;
        float m_weight;
        float m_volume;
        Yeast.Yeast m_yeastInfo;
    }
}
