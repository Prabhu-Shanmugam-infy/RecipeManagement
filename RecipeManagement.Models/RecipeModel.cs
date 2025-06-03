using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManagement.Models
{
    public class RecipeModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required]      
        public string Ingredients { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Instructions { get; set; } = null!;

        [Display(Name = "Preparation Time(In Minutes)")]
        public int CookingTimeInMins { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public int? AuthorId { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }      

        public virtual UserModel? Author { get; set; }

        public virtual CategoryModel? Category { get; set; } 

        public virtual List<string> RecipeImages { get; set; } = new List<String>();
    }
}
