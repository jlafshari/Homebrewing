using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using BeerRecipeCore;
using BeerRecipeCore.Data;
using BeerRecipeCore.Data.Models;
using BeerRecipeCore.Fermentables;
using BeerRecipeCore.Hops;
using BeerRecipeCore.Styles;
using BeerRecipeCore.Yeast;
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
            m_deleteRecipeCommand = new RelayCommand<RecipeDataModel>(DeleteRecipe);
            m_addHopsIngredientToRecipeCommand = new RelayCommand<Hops>(AddHopsIngredient);
            m_addFermentableIngredientToRecipeCommand = new RelayCommand<Fermentable>(AddFermentableIngredient);
            m_changeYeastCommand = new RelayCommand<Yeast>(ChangeYeast);
            m_deleteHopsIngredientCommand = new RelayCommand<IHopsIngredient>(DeleteHopsIngredient);
            m_deleteFermentableIngredientCommand = new RelayCommand<IFermentableIngredient>(DeleteFermentableIngredient);

            // get available ingredients
            List<IngredientTypeBase> allAvailableIngredients = RecipeUtility.GetAvailableIngredients().OrderBy(ingredient => ingredient.Name).ToList();
            m_availableHops = allAvailableIngredients.OfType<Hops>().ToReadOnlyObservableCollection();
            m_availableFermentables = allAvailableIngredients.OfType<Fermentable>().ToReadOnlyObservableCollection();
            m_availableYeasts = allAvailableIngredients.OfType<Yeast>().ToReadOnlyObservableCollection();

            List<Style> beerStyles = RecipeUtility.GetAvailableBeerStyles().OrderBy(style => style.Name).ToList();
            m_availableBeerStyles = beerStyles.ToReadOnlyObservableCollection();
            m_savedRecipes = new ObservableCollection<RecipeDataModel>(RecipeUtility.GetSavedRecipes(beerStyles));

            GetSettings();

            // set the current recipe to the first in the collection
            CurrentRecipe = m_savedRecipes.FirstOrDefault();
        }

        public ObservableCollection<RecipeDataModel> SavedRecipes
        {
            get { return m_savedRecipes; }
            private set
            {
                m_savedRecipes = value;
                RaisePropertyChanged(nameof(SavedRecipes));
            }
        }

        public RecipeDataModel CurrentRecipe
        {
            get { return m_currentRecipe; }
            set
            {
                SaveCurrentRecipe();
                m_currentRecipe = value;
                RaisePropertyChanged(nameof(CurrentRecipe));
                RaisePropertyChanged(nameof(IsRecipeSelected));
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

        public ICommand DeleteRecipeCommand
        {
            get { return m_deleteRecipeCommand; }
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

        public ICommand DeleteHopsIngredientCommand
        {
            get { return m_deleteHopsIngredientCommand; }
        }

        public ICommand DeleteFermentableIngredientCommand
        {
            get { return m_deleteFermentableIngredientCommand; }
        }

        public void GetSettings()
        {
            m_settings = SettingsUtility.GetSavedSettings();
            foreach (RecipeDataModel recipe in m_savedRecipes)
                recipe.ExtractionEfficiency = m_settings.ExtractionEfficiency;
        }

        public void SaveCurrentRecipe()
        {
            if (IsRecipeSelected)
                RecipeUtility.SaveRecipe(CurrentRecipe);
        }

        private void AddNewRecipe()
        {
            SaveCurrentRecipe();
            CurrentRecipe = RecipeUtility.CreateRecipe();
            CurrentRecipe.Name = c_defaultRecipeName;
            CurrentRecipe.Size = m_settings.RecipeSize;
            CurrentRecipe.BoilTime = m_settings.BoilTime;
            CurrentRecipe.ExtractionEfficiency = m_settings.ExtractionEfficiency;
            CurrentRecipe.YeastIngredient.Weight = m_settings.YeastWeight;
            SavedRecipes.Add(CurrentRecipe);
            RaisePropertyChanged(nameof(IsRecipeSelected));
        }

        private void DeleteRecipe(RecipeDataModel recipe)
        {
            RecipeUtility.DeleteRecipe(recipe);

            // set the current recipe to the previous recipe in the collection
            int previousRecipeIndex = SavedRecipes.IndexOf(recipe) - 1;
            CurrentRecipe = previousRecipeIndex == -1 ? SavedRecipes.FirstOrDefault() : SavedRecipes[previousRecipeIndex];

            SavedRecipes.Remove(recipe);
        }

        private void AddHopsIngredient(Hops hops)
        {
            HopsIngredientDataModel hopsIngredient = HopsUtility.CreateHopsIngredient(hops, CurrentRecipe.RecipeId);
            hopsIngredient.Amount = m_settings.HopsAmount;
            hopsIngredient.PropertyChanged += CurrentRecipe.Ingredient_PropertyChanged;
            CurrentRecipe.HopsIngredients.Add(hopsIngredient);
        }

        private void AddFermentableIngredient(Fermentable fermentable)
        {
            FermentableIngredientDataModel fermentableIngredient = FermentableUtility.CreateFermentableIngredient(fermentable, CurrentRecipe.RecipeId);
            fermentableIngredient.PropertyChanged += CurrentRecipe.Ingredient_PropertyChanged;
            CurrentRecipe.FermentableIngredients.Add(fermentableIngredient);
        }

        private void ChangeYeast(Yeast yeastInfo)
        {
            if (CurrentRecipe.YeastIngredient == null)
                CurrentRecipe.YeastIngredient = YeastUtility.CreateYeastIngredient();

            CurrentRecipe.YeastIngredient.YeastInfo = yeastInfo;
            CurrentRecipe.UpdateRecipeOutcome();
        }

        private void DeleteHopsIngredient(IHopsIngredient hopsIngredient)
        {
            CurrentRecipe.HopsIngredients.Remove(hopsIngredient);
            HopsUtility.DeleteHopsIngredient(((HopsIngredientDataModel) hopsIngredient).HopsId);
        }

        private void DeleteFermentableIngredient(IFermentableIngredient fermentableIngredient)
        {
            CurrentRecipe.FermentableIngredients.Remove(fermentableIngredient);
            FermentableUtility.DeleteFermentableIngredient(((FermentableIngredientDataModel) fermentableIngredient).FermentableId);
        }

        const string c_defaultRecipeName = "My Recipe";
        readonly ReadOnlyObservableCollection<Hops> m_availableHops;
        readonly ReadOnlyObservableCollection<Fermentable> m_availableFermentables;
        readonly ReadOnlyObservableCollection<Yeast> m_availableYeasts;
        readonly ReadOnlyObservableCollection<Style> m_availableBeerStyles;
        readonly RelayCommand m_addNewRecipeCommand;
        readonly RelayCommand<RecipeDataModel> m_deleteRecipeCommand;
        readonly RelayCommand<Hops> m_addHopsIngredientToRecipeCommand;
        readonly RelayCommand<Fermentable> m_addFermentableIngredientToRecipeCommand;
        readonly RelayCommand<Yeast> m_changeYeastCommand;
        readonly RelayCommand<IHopsIngredient> m_deleteHopsIngredientCommand;
        readonly RelayCommand<IFermentableIngredient> m_deleteFermentableIngredientCommand;
        ObservableCollection<RecipeDataModel> m_savedRecipes;
        RecipeDataModel m_currentRecipe = null;
        SettingsDataModel m_settings;
    }
}
