using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using BeerRecipeCore;
using BeerRecipeCore.Data;
using BeerRecipeCore.Data.Models;
using MvvmFoundation.Wpf;

namespace HomebrewApp
{
    public sealed class BatchesViewModel : ObservableObject
    {
        public BatchesViewModel()
        {
            m_addNewBatchCommand = new RelayCommand<RecipeDataModel>(AddNewBatch, CanAddNewBatch);
            m_deleteBatchCommand = new RelayCommand<BatchDataModel>(DeleteBatch);
            m_addGravityReadingCommand = new RelayCommand(AddGravityReading, CanAddGravityReading);
            m_deleteGravityReadingCommand = new RelayCommand<GravityReadingDataModel>(DeleteGravityReading);

            List<Style> beerStyles = RecipeUtility.GetAvailableBeerStyles().OrderBy(style => style.Name).ToList();
            m_availableRecipes = new ObservableCollection<RecipeDataModel>(RecipeUtility.GetSavedRecipes(beerStyles));
            m_savedBatches = new ObservableCollection<BatchDataModel>(BatchUtility.GetSavedBatches(m_availableRecipes));

            CurrentBatch = m_savedBatches.FirstOrDefault();
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
                SaveCurrentBatch();
                m_currentBatch = value;
                RaisePropertyChanged("CurrentBatch");
            }
        }

        public ObservableCollection<RecipeDataModel> AvailableRecipes
        {
            get { return m_availableRecipes; }
            private set
            {
                m_availableRecipes = value;
                RaisePropertyChanged("AvailableRecipes");
            }
        }

        public ICommand AddNewBatchCommand
        {
            get { return m_addNewBatchCommand; }
        }

        public ICommand DeleteBatchCommand
        {
            get { return m_deleteBatchCommand; }
        }

        public ICommand AddGravityReadingCommand
        {
            get { return m_addGravityReadingCommand; }
        }

        public ICommand DeleteGravityReadingCommand
        {
            get { return m_deleteGravityReadingCommand; }
        }

        public bool IsBatchSelected
        {
            get { return CurrentBatch != null; }
        }

        public void SaveCurrentBatch()
        {
            if (IsBatchSelected)
                BatchUtility.SaveBatch(CurrentBatch);
        }

        private void AddNewBatch(RecipeDataModel recipe)
        {
            SaveCurrentBatch();
            CurrentBatch = BatchUtility.CreateBatch(recipe);
            SavedBatches.Add(CurrentBatch);
        }

        private bool CanAddNewBatch(RecipeDataModel recipe)
        {
            return recipe != null;
        }

        private void DeleteBatch(BatchDataModel batch)
        {
            BatchUtility.DeleteBatch(batch);

            if (CurrentBatch == batch)
            {
                int previousBatchIndex = SavedBatches.IndexOf(batch) - 1;
                CurrentBatch = previousBatchIndex >= 0 ? SavedBatches[previousBatchIndex] : SavedBatches.FirstOrDefault();
            }

            SavedBatches.Remove(batch);
        }

        private void AddGravityReading()
        {
            GravityReadingDataModel gravityReading = BatchUtility.CreateGravityReading(CurrentBatch.BatchId);
            gravityReading.PropertyChanged += CurrentBatch.Ingredient_PropertyChanged;
            CurrentBatch.RecordedGravityReadings.Add(gravityReading);
        }

        private bool CanAddGravityReading()
        {
            return CurrentBatch != null;
        }

        private void DeleteGravityReading(GravityReadingDataModel gravityReading)
        {
            CurrentBatch.RecordedGravityReadings.Remove(gravityReading);
            BatchUtility.DeleteGravityReading(gravityReading.GravityReadingId);
        }

        readonly RelayCommand<RecipeDataModel> m_addNewBatchCommand;
        readonly RelayCommand<BatchDataModel> m_deleteBatchCommand;
        readonly RelayCommand m_addGravityReadingCommand;
        readonly RelayCommand<GravityReadingDataModel> m_deleteGravityReadingCommand;
        ObservableCollection<BatchDataModel> m_savedBatches;
        ObservableCollection<RecipeDataModel> m_availableRecipes;
        BatchDataModel m_currentBatch = null;
    }
}
