using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RecipeManagement.Contracts;
using RecipeManagement.Entities;
using RecipeManagement.Models;

namespace RecipeManagement.Web.Pages.Recipe
{
    [Authorize(Roles = "User,Admin")]
    public class ManageModel : PageModel
    {

        private readonly RecipeManagementService _service;

       
        public ManageModel(RecipeManagementService service)
        {
            _service = service;
        }

        public IList<RecipeModel> Recipe { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Recipe = await _service.GetAllRecipesAsync() ;
        }
    }
}
