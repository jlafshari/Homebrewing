using BeerRecipeCore;
using MvvmFoundation.Wpf;

namespace BeerRecipeCore.Data.Models
{
    public class HopsIngredientDataModel : ObservableObject, IHopsIngredient
    {
        public HopsIngredientDataModel(Hops hopsInfo, int hopsId)
        {
            m_hopsInfo = hopsInfo;
            m_hopsId = hopsId;
        }

        public int HopsId
        {
            get { return m_hopsId; }
        }

        public float Amount
        {
            get
            {
                return m_amount;
            }
            set
            {
                m_amount = value;
                RaisePropertyChanged("Amount");
            }
        }

        public int Time
        {
            get
            {
                return m_time;
            }
            set
            {
                m_time = value;
                RaisePropertyChanged("Time");
            }
        }

        public HopsFlavorType FlavorType
        {
            get
            {
                return m_flavorType;
            }
            set
            {
                m_flavorType = value;
                RaisePropertyChanged("FlavorType");
            }
        }

        public HopsForm Form
        {
            get
            {
                return m_form;
            }
            set
            {
                m_form = value;
                RaisePropertyChanged("Form");
            }
        }

        public Hops HopsInfo
        {
            get { return m_hopsInfo; }
        }

        int m_hopsId;
        float m_amount;
        int m_time;
        HopsFlavorType m_flavorType;
        HopsForm m_form;
        Hops m_hopsInfo;
    }
}
