using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RecipeApplication.Models;

namespace RecipeApplication
{
    public partial class ScaleRecipeWindow : Window
    {
        private Recipe recipe;

        public ScaleRecipeWindow(Recipe recipe)
        {
            InitializeComponent();
            this.recipe = recipe;
            SelectScalingFactor.ItemsSource = new List<double> { 0.5, 2, 3 };
        }

        //button to scale the recipe
        //https://stackoverflow.com/questions/2675196/c-sharp-method-to-scale-values
        private void Scale_Click(object sender, RoutedEventArgs e)
        {
            if (SelectScalingFactor.SelectedItem != null)
            {
                double scalingFactor = (double)SelectScalingFactor.SelectedItem;
                ScaleRecipe(scalingFactor);
            }
            else
            {
                MessageBox.Show("Please select a scaling factor.");
            }
        }
        //method to scale the recipe
        //https://stackoverflow.com/questions/2675196/c-sharp-method-to-scale-values
        private void ScaleRecipe(double scalingFactor)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                ingredient.Quantity = ingredient.OriginalQuantity * scalingFactor;
            }

            UpdateDisplayedCalories();

            MessageBox.Show($"Recipe scaled by a factor of {scalingFactor}.", "Scaling Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close(); //close the window after scaling
        }

        //method to reset quantities
        //https://stackoverflow.com/questions/32476466/how-to-restore-the-quantity-and-consumption-fields-to-their-original-numbers-in
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                ingredient.ResetQuantity();
            }

            UpdateDisplayedCalories();

            MessageBox.Show("Recipe quantities have been reset to their original values.", "Reset Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close(); //close the window after resetting
        }
        //method to update calories according to the scale
        //https://stackoverflow.com/questions/32476466/how-to-restore-the-quantity-and-consumption-fields-to-their-original-numbers-in
        private void UpdateDisplayedCalories()
        {
            double totalCalories = recipe.Ingredients.Sum(ing => ing.Calories * ing.Quantity / ing.OriginalQuantity);
           
        }

        private void SelectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
