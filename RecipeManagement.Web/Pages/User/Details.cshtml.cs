using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeManagement.Entities;
using RecipeManagement.Models;

namespace RecipeManagement.Web.Pages.User
{
    public class DetailsModel : PageModel
    {
        private readonly RecipeManagementService _service;      

        public DetailsModel(RecipeManagementService service)
        {
            _service = service;
        }

        [BindProperty]
        public UserModel UserModel { get; set; } 

        public async Task<IActionResult> OnGetAsync(int id)
        {            

            var user = await _service.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                UserModel = user;
            }
            return Page();
        }
    }
}
