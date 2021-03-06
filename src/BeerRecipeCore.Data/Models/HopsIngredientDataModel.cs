﻿using BeerRecipeCore.Hops;
using MvvmFoundation.Wpf;

namespace BeerRecipeCore.Data.Models
{
    public class HopsIngredientDataModel : ObservableObject, IHopsIngredient
    {
        public HopsIngredientDataModel(Hops.Hops hopsInfo, int hopsId)
        {
            HopsInfo = hopsInfo;
            HopsId = hopsId;
        }

        public int HopsId { get; }

        public float Amount
        {
            get
            {
                return m_amount;
            }
            set
            {
                m_amount = value;
                RaisePropertyChanged(nameof(Amount));
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
                RaisePropertyChanged(nameof(Time));
            }
        }

        public int? DryHopTime
        {
            get
            {
                return m_dryHopTime;
            }
            set
            {
                m_dryHopTime = value;
                RaisePropertyChanged(nameof(DryHopTime));
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
                RaisePropertyChanged(nameof(FlavorType));
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
                RaisePropertyChanged(nameof(Form));
            }
        }

        public HopsUse Use
        {
            get
            {
                return m_use;
            }
            set
            {
                m_use = value;
                RaisePropertyChanged(nameof(Use));
            }
        }

        public Hops.Hops HopsInfo { get; }

        float m_amount;
        int m_time;
        int? m_dryHopTime;
        HopsFlavorType m_flavorType;
        HopsForm m_form;
        HopsUse m_use;
    }
}
