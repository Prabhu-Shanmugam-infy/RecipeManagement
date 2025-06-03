using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeManagement.Models;

namespace RecipeManagement.Web.Pages
{
    public class registerModel : PageModel
    {
        private readonly RecipeManagementService _service;

        public registerModel(RecipeManagementService service)
        {
            _service = service;
        }
       

        [BindProperty]
        public RegisterModel? RegisterModel { get; set; }
        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                var response = await _service.Register(new Contracts.RegisterRequest() { UserName = RegisterModel.UserName, Email = RegisterModel.Email, Password = RegisterModel.Password });

                return RedirectToPage("./login");
            }

            return Page();

        }
    }
}
