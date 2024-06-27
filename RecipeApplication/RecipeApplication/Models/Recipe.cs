using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RecipeApplication.Models
{
    //Model for recipe
    //https://www.youtube.com/watch?v=fs7m2Qo84d4
    public class Recipe
    {
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<RecipeStep> Steps { get; set; } = new List<RecipeStep>();

        public event Action<double> CaloriesExceeded;
    
    public void CreateRecipe(string name, List<Ingredient> ingredients, List<RecipeStep> steps)
    {
        Name = name;
        Ingredients = ingredients;
        Steps = steps;

        CheckCalories();
    }

    public void CheckCalories()
    {
        double totalCalories = Ingredients.Sum(ing => ing.Calories * ing.Quantity / ing.OriginalQuantity);
        if (totalCalories > 300)
        {
            CaloriesExceeded?.Invoke(totalCalories);
        }
    }
        public void ClearRecipe()
        {
            Ingredients.Clear();
            Steps.Clear();
        }
    }
}

