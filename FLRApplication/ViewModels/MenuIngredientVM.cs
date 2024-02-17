using System.Collections.Generic;
using FLRApplication.Models;

namespace FLRApplication.ViewModels
{
    public class MenuIngredientVM
    {
        public MenuItem MenuItem { get; set; }

        public Ingredient Ingredient { get; set; }

        public IEnumerable<Ingredient> ListIngredients { get; set;}
    }
}