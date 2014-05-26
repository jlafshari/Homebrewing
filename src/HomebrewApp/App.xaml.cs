using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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

            base.OnDeactivated(e);
        }
    }
}
