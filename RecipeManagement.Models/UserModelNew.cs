using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManagement.Models
{
    public class UserModelNew : UserModel
    {
        [Display(Name = "Profile Picture")]
        public IFormFile? FormFile { get; set; }
    }
}
