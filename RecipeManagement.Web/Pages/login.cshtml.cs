using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using RecipeManagement.Models;
using System.Security.Claims;

namespace RecipeManagement.Web.Pages
{
    public class loginModel : PageModel
    {
        private readonly RecipeManagementService _service;

        public loginModel(RecipeManagementService service)
        {
            _service = service;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public LoginModel? LoginModel { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var response = _service.Login(new Contracts.LoginRequest() { Email = LoginModel.Email, Password = LoginModel.Password });
                if (response != null && response.Result != null && response.Result.Token != null)
                {
                    HttpContext.Session.SetString("Token", response.Result.Token);
                    var claimsPrincipal = ClaimsHelper.GetClaimsPrincipalFromJwt(response.Result.Token);

                    if (claimsPrincipal != null)
                    {
                        var claims1 = claimsPrincipal.Claims;
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name,  claims1.FirstOrDefault(c => c.Type == "sub").Value),
                            new Claim(ClaimTypes.Role, claims1.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value ),
                            new Claim("UserId",  claims1.FirstOrDefault(c => c.Type == "UserId").Value)
                        };
                        var claimsIdentity = new ClaimsIdentity(claims, "cookie");
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                        return LocalRedirect("/Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("error", "Invalid User Credentials.");
                }
            }

            return Page();

        }


       
    }
}
