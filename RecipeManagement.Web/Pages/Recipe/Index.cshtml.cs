using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeManagement.Contracts;
using RecipeManagement.Entities;
using RecipeManagement.Models;

namespace RecipeManagement.Web.Pages.Recipe
{
    public class IndexModel : PageModel
    {

        private readonly RecipeManagementService _service;


        public IndexModel(RecipeManagementService service)
        {
            _service = service;
        }

        public IList<RecipeModel> Recipe { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            if (this.User.IsInRole("Admin"))
            {
                Recipe = await _service.GetAllRecipesAsync();
            }
            else
            {
                Recipe = await _service.GetAllRecipesAsync();
                Recipe = Recipe.Where(r => r.AuthorId == userId).ToList();
            }

        }
    }
}
