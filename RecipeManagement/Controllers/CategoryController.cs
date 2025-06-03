using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeManagement.Contracts;
using RecipeManagement.Entities;
using RecipeManagement.Models;

namespace RecipeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private IConfiguration _config;
        private readonly RecipeContext _context;
        public CategoryController(IConfiguration config, RecipeContext context)
        {
            _config = config;
            _context = context;
        }


        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public ActionResult<List<CategoryModel>> GetAll()
        {
            return _context.Categories.Select(c => new CategoryModel() { Id= c.Id , Name= c.Name  }).ToList();
        }
    }
}
