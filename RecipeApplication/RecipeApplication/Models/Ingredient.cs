using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApplication.Models
{
    //create the ingredients model
    //https://www.youtube.com/watch?v=fs7m2Qo84d4
    public class Ingredient
    {
        public string Name { get; set; }
        public double OriginalQuantity { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public double Calories { get; set; }
        public string FoodGroup { get; set; }

        public void ResetQuantity()
        {
            Quantity = OriginalQuantity;
        }
    }
}