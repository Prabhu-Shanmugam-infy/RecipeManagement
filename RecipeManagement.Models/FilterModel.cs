using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManagement.Models
{
   public class FilterModel
    {

        public string? Ingredients { get; set; }

        [Display(Name = "Max. Preparation Time(In Mins)")]
        public int? CookingTimeInMins { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
    }
}
