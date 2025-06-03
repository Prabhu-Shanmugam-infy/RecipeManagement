using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Policy;

namespace RecipeManagement.Web.Pages
{
    public class logoutModel : PageModel
    {
        public IActionResult OnGetAsync()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // RedirectToPage("login");
            // Response.Redirect("./login");
            //return new RedirectToPageResult("/Login");
            return Redirect("/");

        }
    }
}
