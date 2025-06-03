using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RecipeManagement.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string? Password { get; set; }

        [Display(Name = "Profile Picture")]
        public string? ProfilePicture { get; set; }

      

        [Display(Name = "Biography")]
        public string? Bio { get; set; }


        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [Display(Name = "Is Admin")]
        public bool IsAdmin { get; set; }

    }
}
