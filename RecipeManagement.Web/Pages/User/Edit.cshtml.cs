using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using RecipeManagement.Entities;
using RecipeManagement.Models;

namespace RecipeManagement.Web.Pages.User
{
    public class EditModel : PageModel
    {
        private readonly RecipeManagementService _service;

        public EditModel(RecipeManagementService service)
        {
            _service = service;
        }


        [BindProperty]
        public UserModel UserModel { get; set; } = default!;
        
        [BindProperty]
        [Display(Name = "Profile Picture")]
        public IFormFile? FormFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user =  await _service.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UserModel = user;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
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

            var response = await _service.UpdateUserAsync(UserModel);

            return RedirectToPage("./Index");
        }

       
    }
}
