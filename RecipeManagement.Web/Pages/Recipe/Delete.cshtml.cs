using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeManagement.Entities;
using RecipeManagement.Models;

namespace RecipeManagement.Web.Pages.Recipe
{
    [Authorize(Roles = "User,Admin")]
    public class DeleteModel : PageModel
    {
        private readonly RecipeManagementService _service;

       
        public DeleteModel(RecipeManagementService service)
        {
            _service = service;
        }

        [BindProperty]
        public RecipeModel RecipeModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var recipe = await _service.GetRecipeByIdAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            RecipeModel = recipe;
            return Page();
            
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _service.DeleteRecipeAsync(id);
            return RedirectToPage("./Index");
        }
    }
}
