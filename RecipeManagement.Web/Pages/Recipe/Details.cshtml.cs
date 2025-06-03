using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeManagement.Models;

namespace RecipeManagement.Web.Pages.Recipe
{
    [Authorize(Roles = "User,Admin")]
    public class DetailsModel : PageModel
    {
        private readonly RecipeManagementService _service;
        
        public DetailsModel(RecipeManagementService service)
        {
            _service = service;
        }

        public RecipeModel Recipe { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        { 
            var recipe = await _service.GetRecipeByIdAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            Recipe = recipe;
            return Page();

          
            
        }
    }
}
