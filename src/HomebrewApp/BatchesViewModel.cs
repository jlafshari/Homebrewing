using System.Collections.ObjectModel;
using System.Windows.Input;
using BeerRecipeCore.Data.Models;
using MvvmFoundation.Wpf;

namespace HomebrewApp
{
    public sealed class BatchesViewModel : ObservableObject
    {
        public BatchesViewModel()
        {
            m_addNewBatchCommand = new RelayCommand(AddNewBatch);
        }

        public ObservableCollection<BatchDataModel> SavedBatches
        {
            get { return m_savedBatches; }
            set
            {
                m_savedBatches = value;
                RaisePropertyChanged("SavedBatches");
            }
        }

        public BatchDataModel CurrentBatch
        {
            get { return m_currentBatch; }
            set
            {
                m_currentBatch = value;
                RaisePropertyChanged("CurrentBatch");
            }
        }

        public ICommand AddNewBatchCommand
        {
            get { return m_addNewBatchCommand; }
        }

        private void AddNewBatch()
        {
            //CurrentBatch = BatchUtility.CreateBatch
        }

        readonly RelayCommand m_addNewBatchCommand;
        ObservableCollection<BatchDataModel> m_savedBatches;
        BatchDataModel m_currentBatch = null;
    }
}
