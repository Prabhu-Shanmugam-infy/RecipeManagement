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
