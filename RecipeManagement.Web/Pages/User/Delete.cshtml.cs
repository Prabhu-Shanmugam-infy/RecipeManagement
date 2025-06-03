using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeManagement.Models;

namespace RecipeManagement.Web.Pages.User
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly RecipeManagementService _service;

        public DeleteModel(RecipeManagementService service)
        {
            _service = service;
        }

        [BindProperty]
        public UserModel UserModel { get; set; } = default!;

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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _service.DeleteUserAsync(id);

            return RedirectToPage("./Index");
        }
    }
}
