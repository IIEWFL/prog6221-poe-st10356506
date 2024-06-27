using System.Windows;
using System.Windows.Controls;
using RecipeApplication.Models;

namespace RecipeApplication
{
    public partial class RecipeWindow : Window
    {
        public Recipe recipe;

        public RecipeWindow(Recipe recipe)
        {
            InitializeComponent();
            this.recipe = recipe;

            if (!string.IsNullOrEmpty(recipe.Name))
            {
                RecipeNameTextBox.Text = recipe.Name;
                foreach (var ingredient in recipe.Ingredients)
                {
                    AddIngredientPanel(ingredient);
                }
                foreach (var step in recipe.Steps)
                {
                    AddStepPanel(step);
                }
            }
        }
        //method to add ingredients when button is click
        //https://www.simplilearn.com/tutorials/asp-dot-net-tutorial/c-sharp-list
        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            AddIngredientPanel(new Ingredient());
        }

        private void AddIngredientPanel(Ingredient ingredient)
        {
            var ingredientPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };

            //labels for each input field
            //https://www.c-sharpcorner.com/UploadFile/mahesh/using-xaml-label-in-wpf/
            var nameLabel = new Label { Content = "Name:", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(5) };
            var nameTextBox = new TextBox { Text = ingredient.Name, Width = 100, Margin = new Thickness(5) };

            var quantityLabel = new Label { Content = "Quantity:", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(5) };
            var quantityTextBox = new TextBox { Text = ingredient.Quantity.ToString(), Width = 50, Margin = new Thickness(5) };

            var unitLabel = new Label { Content = "Unit:", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(5) };
            var unitTextBox = new TextBox { Text = ingredient.Unit, Width = 50, Margin = new Thickness(5) };

            var caloriesLabel = new Label { Content = "Calories:", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(5) };
            var caloriesTextBox = new TextBox { Text = ingredient.Calories.ToString(), Width = 50, Margin = new Thickness(5) };

            var foodGroupLabel = new Label { Content = "Food Group:", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(5) };
            var foodGroupComboBox = new ComboBox { Width = 100, Margin = new Thickness(5) };
            foodGroupComboBox.ItemsSource = new[] { "Vegetables", "Fruits", "Proteins", "Grains", "Dairy" };
            foodGroupComboBox.SelectedItem = ingredient.FoodGroup;

            ingredientPanel.Children.Add(nameLabel);
            ingredientPanel.Children.Add(nameTextBox);
            ingredientPanel.Children.Add(quantityLabel);
            ingredientPanel.Children.Add(quantityTextBox);
            ingredientPanel.Children.Add(unitLabel);
            ingredientPanel.Children.Add(unitTextBox);
            ingredientPanel.Children.Add(caloriesLabel);
            ingredientPanel.Children.Add(caloriesTextBox);
            ingredientPanel.Children.Add(foodGroupLabel);
            ingredientPanel.Children.Add(foodGroupComboBox);

            IngredientsPanel.Children.Add(ingredientPanel);
        }
        //button to add steps
        //https://stackoverflow.com/questions/67248214/how-to-create-a-click-handler-for-my-xaml-button
        private void AddStep_Click(object sender, RoutedEventArgs e)
        {
            AddStepPanel(new RecipeStep());
        }

        
        private void AddStepPanel(RecipeStep step)
        {
            var stepTextBox = new TextBox { Text = step.Step, Margin = new Thickness(0, 5, 0, 5), Width = 400 };
            StepsPanel.Children.Add(stepTextBox);
        }

        //method to save the recipe
        //https://stackoverflow.com/questions/47111748/how-to-save-values-to-a-list-in-c-sharp
        private void SaveRecipe_Click(object sender, RoutedEventArgs e)
        {
            //check if RecipeNameTextBox is not null
            if (!string.IsNullOrEmpty(RecipeNameTextBox.Text))
            {
                recipe.Name = RecipeNameTextBox.Text;
                recipe.Ingredients.Clear();

                // save each ingredient of the recipe
                foreach (StackPanel panel in IngredientsPanel.Children)
                {
                    var nameTextBox = panel.Children[1] as TextBox;
                    var quantityTextBox = panel.Children[3] as TextBox;
                    var unitTextBox = panel.Children[5] as TextBox;
                    var caloriesTextBox = panel.Children[7] as TextBox;
                    var foodGroupComboBox = panel.Children[9] as ComboBox;

                    //check if any of the ingredient fields are empty
                    if (string.IsNullOrEmpty(nameTextBox.Text) ||
                        string.IsNullOrEmpty(quantityTextBox.Text) ||
                        string.IsNullOrEmpty(unitTextBox.Text) ||
                        string.IsNullOrEmpty(caloriesTextBox.Text) ||
                        foodGroupComboBox.SelectedItem == null)
                    {
                        MessageBox.Show("Please complete all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return; //if there are empty fields, the above message will be displayed
                    }

                    //error message if theres an invalid quantity
                    if (!double.TryParse(quantityTextBox.Text, out double quantity) || quantity <= 0)
                    {
                        MessageBox.Show("Invalid quantity entered. Please enter a valid number greater than zero.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (!double.TryParse(caloriesTextBox.Text, out double calories) || calories <= 0)
                    {
                        MessageBox.Show("Invalid calories entered. Please enter a valid number greater than zero.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    //create new ingredient and add to recipe
                    var ingredient = new Ingredient
                    {
                        Name = nameTextBox.Text,
                        OriginalQuantity = quantity, 
                        Quantity = quantity,
                        Unit = unitTextBox.Text,
                        Calories = calories,
                        FoodGroup = foodGroupComboBox.SelectedItem.ToString()
                    };

                    recipe.Ingredients.Add(ingredient);
                }

                //clear previous steps
                recipe.Steps.Clear();

                //save steps
                //https://stackoverflow.com/questions/47111748/how-to-save-values-to-a-list-in-c-sharp
                foreach (TextBox textBox in StepsPanel.Children)
                {
                    recipe.Steps.Add(new RecipeStep { Step = textBox.Text });
                }

                //update ListBox and close window
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please enter a recipe name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}