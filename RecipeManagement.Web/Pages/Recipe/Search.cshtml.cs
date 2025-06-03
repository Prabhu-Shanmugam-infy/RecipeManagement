using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeManagement.Models;

namespace RecipeManagement.Web.Pages.Recipe
{
    public class SearchModel : PageModel
    {
        private readonly RecipeManagementService _service;

        public SearchModel(RecipeManagementService service)
        {
            _service = service;
        }

        public IList<RecipeModel> Recipe { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Recipe = await _service.SearchRecipesAsync(Request.Query["q"].ToString());
        }
      
    }
}
