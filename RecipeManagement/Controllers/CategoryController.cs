using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeManagement.Contracts;
using RecipeManagement.Entities;
using RecipeManagement.Interface;
using RecipeManagement.Models;

namespace RecipeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private IConfiguration _config;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(IConfiguration config, ICategoryRepository categoryRepository)
        {
            _config = config;            
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public ActionResult<List<CategoryModel>> GetAll()
        {
            return _categoryRepository.GetAllCategories();
        }
    }
}
