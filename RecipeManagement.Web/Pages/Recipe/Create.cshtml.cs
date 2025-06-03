using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using RecipeManagement.Contracts;
using RecipeManagement.Entities;
using RecipeManagement.Models;

namespace RecipeManagement.Web.Pages.Recipe
{
    [Authorize]
    public class CreateModel : PageModel
    {

        private readonly RecipeManagementService _service;

        public CreateModel(RecipeManagementService service)
        {
            _service = service;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var items = _service.GetAllCategoriesAsync().Result;
            var selItems = new SelectList(items, "Id", "Name");

            ViewData["CategoryId"] = selItems;
            return Page();
        }

        [BindProperty]
        public RecipeModel RecipeModel { get; set; } = default!;


        [Display(Name = "Recipe Images")]
        [BindProperty]
        public List<IFormFile> FormFile { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var images = new List<string>();

            foreach (var Upload in FormFile)
            {
                var newfilename = System.Guid.NewGuid() + Path.GetExtension(Upload.FileName);

                var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/images", newfilename);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await Upload.CopyToAsync(fileStream);
                }
                var thumbnailPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/thumbnail", newfilename);
                ThumbnailGenerator.GenerateThumbnail(file, 50, 50, thumbnailPath);
                images.Add(newfilename);
            }

            RecipeModel.RecipeImages = images;

            await _service.CreateRecipeAsync(RecipeModel); ;

            return RedirectToPage("./Index");
        }
    }
}
