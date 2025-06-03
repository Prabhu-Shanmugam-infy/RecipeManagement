using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeManagement.Models;

namespace RecipeManagement.Web.Pages.User
{
    [Authorize(Roles = "User,Admin")]
    public class IndexModel : PageModel
    {
        private readonly RecipeManagementService _service;

        public IndexModel(RecipeManagementService service)
        {
            _service = service;
        }

        public IList<UserModel> UserModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            UserModel = await _service.GetAllUsersAsync();
        }
    }
}
