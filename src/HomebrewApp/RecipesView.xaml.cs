using System.Windows.Controls;

namespace HomebrewApp
{
    /// <summary>
    /// Interaction logic for RecipesView.xaml
    /// </summary>
    public partial class RecipesView : UserControl
    {
        public RecipesView()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RecipesViewModel viewModel = (RecipesViewModel) DataContext;
            viewModel.CurrentRecipe.UpdateRecipeOutcome();
        }
    }
}
