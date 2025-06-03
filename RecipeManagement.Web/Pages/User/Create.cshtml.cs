using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace RecipeManagement.Web.Pages.User
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly RecipeManagementService _service;
        
        public CreateModel(RecipeManagementService service)
        {
            _service = service;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        [Display(Name = "Profile Picture")]
        public IFormFile? FormFile { get; set; }

        [BindProperty]
        public UserModel UserModel { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (FormFile != null)
            {
                var newfilename = System.Guid.NewGuid() + Path.GetExtension(FormFile.FileName);

                var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/images", newfilename);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await FormFile.CopyToAsync(fileStream);
                }
                var thumbnailPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/thumbnail", newfilename);
                ThumbnailGenerator.GenerateThumbnail(file, 50, 50, thumbnailPath);
                UserModel.ProfilePicture = newfilename;
            }

            var response = await _service.CreateUserAsync(UserModel);

            return RedirectToPage("./Index");
        }
    }
}
