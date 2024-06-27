using RecipeApplication.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecipeApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Recipe> recipes = new List<Recipe>();
        public MainWindow()
        {
            InitializeComponent();
            //populate the food group combo box
            FilterFoodGroupComboBox.ItemsSource = new List<string> { "Vegetables", "Fruits", "Proteins", "Grains", "Dairy" };
        }
        //method for the create button
        //https://stackoverflow.com/questions/67248214/how-to-create-a-click-handler-for-my-xaml-button
        private void CreateNewRecipe_Click(object sender, RoutedEventArgs e)
        {
            Recipe newRecipe = new Recipe();
            RecipeWindow recipeWindow = new RecipeWindow(newRecipe);
            if (recipeWindow.ShowDialog() == true)
            {
                recipes.Add(newRecipe);
            }
            RefreshRecipeList();
        }
        //display recipe button logic
        //https://stackoverflow.com/questions/32367552/c-sharp-accessing-a-list-from-multiple-methods
        private void DisplayRecipes_Click(object sender, RoutedEventArgs e)
        {
            if (RecipeListBox.SelectedItem is Recipe selectedRecipe)
            {
                DisplayRecipe displayRecipeWindow = new DisplayRecipe(selectedRecipe);
                displayRecipeWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a recipe to display");
            }
            
        }
        //scale recipe method button
        //https://stackoverflow.com/questions/2675196/c-sharp-method-to-scale-values
        private void ScaleRecipe_Click(object sender, RoutedEventArgs e)
        {
            var selectedRecipe = (Recipe)RecipeListBox.SelectedItem;
            if (selectedRecipe != null)
            {
                var scaleRecipeWindow = new ScaleRecipeWindow(selectedRecipe);
                scaleRecipeWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a recipe to scale.");
            }
        }
        //reset quantites method for the button
        private void ResetQuantities_Click(object sender, RoutedEventArgs e)
        //https://stackoverflow.com/questions/39495995/c-sharp-reset-an-array-to-its-initialized-values
        {
            Recipe selectedRecipe = RecipeListBox.SelectedItem as Recipe;

            if (selectedRecipe != null)
            {
                foreach (var ingredient in selectedRecipe.Ingredients)
                {
                    ingredient.Quantity = ingredient.OriginalQuantity;
                    MessageBox.Show("Quantities reset successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                RefreshRecipeList(); 
            }
            else
            {
                MessageBox.Show("Please select a recipe to reset quantities.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //clear recipe button method
        //https://stackoverflow.com/questions/39495995/c-sharp-reset-an-array-to-its-initialized-values
        private void ClearRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (RecipeListBox.SelectedItem is Recipe selectedRecipe)
            {
                selectedRecipe.ClearRecipe();
                recipes.Remove(selectedRecipe);
                RefreshRecipeList();
                MessageBox.Show("Recipe cleared successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please select a recipe to clear.");
            }
        }
        //apply filter button method
        public void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {
           
                try
                {
                  
                    var filteredRecipes = recipes.ToList();

                    //filter by ingredient name
                    if (!string.IsNullOrEmpty(FilterIngredientTextBox.Text))
                    {
                        string filterIngredientName = FilterIngredientTextBox.Text.Trim().ToLower();
                        filteredRecipes = filteredRecipes
                            .Where(r => r.Ingredients.Any(i => i.Name.ToLower().Contains(filterIngredientName)))
                            .ToList();
                    }

                    //filter by food group
                    if (FilterFoodGroupComboBox.SelectedItem != null)
                    {
                        string selectedFoodGroup = FilterFoodGroupComboBox.SelectedItem.ToString();
                        filteredRecipes = filteredRecipes
                            .Where(r => r.Ingredients.Any(i => i.FoodGroup.Equals(selectedFoodGroup, StringComparison.OrdinalIgnoreCase)))
                            .ToList();
                    }

                    //filter by maximum calories
                    if (!string.IsNullOrEmpty(FilterCaloriesTextBox.Text) && double.TryParse(FilterCaloriesTextBox.Text, out double maxCalories))
                    {
                        filteredRecipes = filteredRecipes
                            .Where(r => r.Ingredients.Sum(i => i.Calories * (i.Quantity / i.OriginalQuantity)) <= maxCalories)
                            .ToList();
                    }

                    //update RecipeListBox with filtered recipes
                    RecipeListBox.ItemsSource = filteredRecipes.OrderBy(r => r.Name).ToList();

                //provide messages to indicate updates
                    MessageBox.Show("Filter has been applied", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                //message if theres an error
                    MessageBox.Show($"An error occurred while applying filters: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


        //method to remove the filters button
        private void RemoveFilters_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //clear filter text boxes
                FilterIngredientTextBox.Text = string.Empty;
                FilterFoodGroupComboBox.SelectedItem = null;
                FilterCaloriesTextBox.Text = string.Empty;

                //display all recipes again
                RecipeListBox.ItemsSource = recipes.OrderBy(r => r.Name).ToList();

                //message to show that the filter has been removed
                MessageBox.Show("Filter removed", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            //message to show that theres an error when removing the filter, error handled 
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while removing filters: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterFoodGroupComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void RecipeListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
          
        }
        //method to refresh the recipelist
        public void RefreshRecipeList()
        {
            RecipeListBox.ItemsSource = recipes.OrderBy(r => r.Name).ToList();
        }

    }
}