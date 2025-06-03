using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RecipeManagement.Web.Pages
{
    public class logoutModel : PageModel
    {
        public IActionResult OnGetAsync()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);            
            return Redirect("/");

        }
    }
}
