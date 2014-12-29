using System;
using System.Windows;

namespace HomebrewApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnDeactivated(EventArgs e)
        {
            // save the current recipe before shutting down
            RecipesView recipesView = (RecipesView) MainWindow.FindName("RecipesView");
            RecipesViewModel recipesViewModel = (RecipesViewModel) recipesView.DataContext;
            recipesViewModel.SaveCurrentRecipe();

            // save the current batch before shutting down
            BatchesView batchesView = (BatchesView) MainWindow.FindName("BatchesView");
            BatchesViewModel batchesViewModel = (BatchesViewModel) batchesView.DataContext;
            batchesViewModel.SaveCurrentBatch();

            base.OnDeactivated(e);
        }
    }
}
