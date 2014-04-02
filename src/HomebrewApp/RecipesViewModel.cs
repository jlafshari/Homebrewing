using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using BeerRecipeCore;
using BeerRecipeCore.Data;
using BeerRecipeCore.Data.Models;
using MvvmFoundation.Wpf;
using Utility;

namespace HomebrewApp
{
    public sealed class RecipesViewModel : ObservableObject
    {
        public RecipesViewModel()
        {
            // initialize commands
            m_addNewRecipeCommand = new RelayCommand(AddNewRecipe);
            m_addHopsIngredientToRecipeCommand = new RelayCommand<Hops>(AddHopsIngredient);
            m_addFermentableIngredientToRecipeCommand = new RelayCommand<Fermentable>(AddFermentableIngredient);
            m_changeYeastCommand = new RelayCommand<Yeast>(ChangeYeast);

            // get available ingredients
            List<IngredientTypeBase> allAvailableIngredients = RecipeUtility.GetAvailableIngredients().ToList();
            m_availableHops = allAvailableIngredients.OfType<Hops>().ToReadOnlyObservableCollection();
            m_availableFermentables = allAvailableIngredients.OfType<Fermentable>().ToReadOnlyObservableCollection();
            m_availableYeasts = allAvailableIngredients.OfType<Yeast>().ToReadOnlyObservableCollection();

            List<Style> beerStyles = RecipeUtility.GetAvailableBeerStyles().ToList();
            m_availableBeerStyles = beerStyles.ToReadOnlyObservableCollection();
            m_savedRecipes = new ObservableCollection<RecipeDataModel>(RecipeUtility.GetSavedRecipes(beerStyles));
        }

        public ObservableCollection<RecipeDataModel> SavedRecipes
        {
            get { return m_savedRecipes; }
            private set
            {
                m_savedRecipes = value;
                RaisePropertyChanged("SavedRecipes");
            }
        }

        public RecipeDataModel CurrentRecipe
        {
            get { return m_currentRecipe; }
            set
            {
                SaveCurrentRecipe();
                m_currentRecipe = value;
                RaisePropertyChanged("CurrentRecipe");
                RaisePropertyChanged("IsRecipeSelected");
            }
        }

        public ReadOnlyObservableCollection<Hops> AvailableHops
        {
            get { return m_availableHops; }
        }

        public ReadOnlyObservableCollection<Fermentable> AvailableFermentables
        {
            get { return m_availableFermentables; }
        }

        public ReadOnlyObservableCollection<Yeast> AvailableYeasts
        {
            get { return m_availableYeasts; }
        }

        public ReadOnlyObservableCollection<Style> AvailableBeerStyles
        {
            get { return m_availableBeerStyles; }
        }

        public bool IsRecipeSelected
        {
            get { return CurrentRecipe != null; }
        }

        public ICommand AddNewRecipeCommand
        {
            get { return m_addNewRecipeCommand; }
        }

        public ICommand AddHopsIngredientToRecipeCommand
        {
            get { return m_addHopsIngredientToRecipeCommand; }
        }

        public ICommand AddFermentableIngredientToRecipeCommand
        {
            get { return m_addFermentableIngredientToRecipeCommand; }
        }

        public ICommand ChangeYeastCommand
        {
            get { return m_changeYeastCommand; }
        }

        private void SaveCurrentRecipe()
        {
            if (IsRecipeSelected)
                RecipeUtility.SaveRecipe(CurrentRecipe);
        }

        private void AddNewRecipe()
        {
            SaveCurrentRecipe();
            CurrentRecipe = RecipeUtility.CreateRecipe();
            CurrentRecipe.Name = c_defaultRecipeName;
            SavedRecipes.Add(CurrentRecipe);
            RaisePropertyChanged("IsRecipeSelected");
        }

        private void AddHopsIngredient(Hops hops)
        {
            HopsIngredientDataModel hopsIngredient = HopsUtility.CreateHopsIngredient(hops, CurrentRecipe.RecipeId);
            CurrentRecipe.HopsIngredients.Add(hopsIngredient);
        }

        private void AddFermentableIngredient(Fermentable fermentable)
        {
            FermentableIngredientDataModel fermentableIngredient = FermentableUtility.CreateFermentableIngredient(fermentable, CurrentRecipe.RecipeId);
            CurrentRecipe.FermentableIngredients.Add(fermentableIngredient);
        }

        private void ChangeYeast(Yeast yeastInfo)
        {
            CurrentRecipe.YeastIngredient.YeastInfo = yeastInfo;
        }

        const string c_defaultRecipeName = "My Recipe";
        readonly ReadOnlyObservableCollection<Hops> m_availableHops;
        readonly ReadOnlyObservableCollection<Fermentable> m_availableFermentables;
        readonly ReadOnlyObservableCollection<Yeast> m_availableYeasts;
        readonly ReadOnlyObservableCollection<Style> m_availableBeerStyles;
        readonly RelayCommand m_addNewRecipeCommand;
        readonly RelayCommand<Hops> m_addHopsIngredientToRecipeCommand;
        readonly RelayCommand<Fermentable> m_addFermentableIngredientToRecipeCommand;
        readonly RelayCommand<Yeast> m_changeYeastCommand;
        ObservableCollection<RecipeDataModel> m_savedRecipes;
        RecipeDataModel m_currentRecipe = null;
    }
}
