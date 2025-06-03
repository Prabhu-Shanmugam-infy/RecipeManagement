using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecipeManagement.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace RecipeManagement.Web.Pages.Recipe
{
    [Authorize(Roles = "User,Admin")]
    public class EditModel : PageModel
    {
        private readonly RecipeManagementService _service;
        public EditModel(RecipeManagementService service)
        {
            _service = service;
        }

        [BindProperty]
        public RecipeModel recipeModel { get; set; } = default!;

        [BindProperty]
        public string? DeletedImages { get; set; }

        [Display(Name = "Recipe Images")]
        [BindProperty]
        public List<IFormFile>? FormFile { get; set; }


        [BindProperty]
        public string RecipeImages { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var items = _service.GetAllCategoriesAsync().Result;
            var selItems = new SelectList(items, "Id", "Name");

            ViewData["CategoryId"] = selItems;
            recipeModel = await _service.GetRecipeByIdAsync(id);
            RecipeImages = String.Join(",", recipeModel.RecipeImages);
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            if (this.User.IsInRole("Admin") || recipeModel.AuthorId == userId)
            {
                return Page();
            }
            else
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
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

            recipeModel.RecipeImages = RecipeImages.Split(",").ToList(); ;
            recipeModel.RecipeImages.AddRange(images);
            if (!string.IsNullOrEmpty(DeletedImages))
            {
                foreach (var img in DeletedImages.Split(","))
                {
                    recipeModel.RecipeImages.Remove(img);
                }
            }
            await _service.UpdateRecipeAsync(recipeModel); ;

            return RedirectToPage("./Index");
        }


    }
}
