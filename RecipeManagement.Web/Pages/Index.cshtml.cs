using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeManagement.Models;

namespace RecipeManagement.Web.Pages;

[Authorize(Roles = "Admin,User")]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    private readonly RecipeManagementService _service;




    [BindProperty]
    public FilterModel filterModel { get; set; } = default!;

    public IList<RecipeModel> Recipe { get; set; } = default!;

    public IndexModel(ILogger<IndexModel> logger, RecipeManagementService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task OnGetAsync()
    {
        if (!User.Identity.IsAuthenticated)
        {
            //Response.Redirect("./login");
             RedirectToPage("./login");
        }

        var items = _service.GetAllCategoriesAsync().Result;
        var selItems = new SelectList(items, "Id", "Name");

        ViewData["CategoryId"] = selItems;
        Recipe = await _service.SearchRecipesAsync("");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }


        Recipe = await _service.FilterRecipeAsync(filterModel);

        var items = _service.GetAllCategoriesAsync().Result;
        var selItems = new SelectList(items, "Id", "Name");

        ViewData["CategoryId"] = selItems;

        return Page();


    }
}
