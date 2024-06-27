using RecipeApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RecipeApplication
{
//displays the recipe onto the window
    public partial class DisplayRecipe : Window
    {
        private Recipe recipe;

        public DisplayRecipe(Recipe recipe)
        {
            InitializeComponent();
            this.recipe = recipe;
            DisplayRecipeDetails(recipe);
        }

        //method to display details of the recipe
        //https://stackoverflow.com/questions/32367552/c-sharp-accessing-a-list-from-multiple-methods
        public void DisplayRecipeDetails(Recipe recipe)
        {
            RecipeName.Text = recipe.Name;

            //clear the ingredients box and populate it
            IngredientsListBox.Items.Clear();
            foreach (var ingredient in recipe.Ingredients)
            {
                IngredientsListBox.Items.Add($"{ingredient.Quantity} {ingredient.Unit} of {ingredient.Name} ({ingredient.Calories} calories)");
            }

            StepsListBox.Items.Clear();
            for (int i = 0; i < recipe.Steps.Count; i++)
            {
                StepsListBox.Items.Add($"{i + 1}. {recipe.Steps[i].Step}");
            }

            //calculate the total calories and display a message if the calories are over 300
            double totalCalories = recipe.Ingredients.Sum(ingredient => ingredient.Calories * (ingredient.Quantity / ingredient.OriginalQuantity));
            if (totalCalories > 300)
            {
                CaloriesWarning.Text = $"Warning: Total calories exceed the limit! Total Calories: {totalCalories}";
                CaloriesWarning.Visibility = Visibility.Visible;
            }
        }
    }
}
