using BeerRecipeCore;
using MvvmFoundation.Wpf;

namespace BeerRecipeCore.Data.Models
{
    public class HopsIngredientDataModel : ObservableObject, IHopsIngredient
    {
        public HopsIngredientDataModel(Hops hopsInfo)
        {
            m_hopsInfo = hopsInfo;
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

        private float m_amount;
        private int m_time;
        private HopsFlavorType m_flavorType;
        private HopsForm m_form;
        private Hops m_hopsInfo;
    }
}
