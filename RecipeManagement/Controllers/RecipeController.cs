using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeManagement.Contracts;
using RecipeManagement.Entities;
using RecipeManagement.Interface;
using RecipeManagement.Models;
using System.Net;

namespace RecipeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private IConfiguration _config;
        
        private readonly IMapper _mapper;
        private readonly IRecipeRepository _recipeRepository;

        public RecipeController(IConfiguration config,  IMapper mapper, IRecipeRepository recipeRepository)
        {
            _config = config;           
            _mapper = mapper;
            _recipeRepository = recipeRepository;
        }

        // GET: api/Recipes
        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public List<RecipeModel> GetRecipesAsync()
        {
            return _recipeRepository.GetAllRecipes();
        }

        [HttpGet("search/{query?}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IEnumerable<RecipeModel>> SearchRecipesAsync(string? query)
        {
            var result = _recipeRepository.GetAllRecipes().Where(r => r.Active == true);
            if (!string.IsNullOrWhiteSpace(query))
            {
                result = result.Where(r => r.Ingredients.ToLower().Contains(query.ToLower())
                || r.Title.ToLower().Contains(query.ToLower())
                || r.Instructions.ToLower().Contains(query.ToLower())).ToList();
            }

            return result;
        }

        [HttpPost("filter")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IEnumerable<RecipeModel>> FilerRecipesAsync(FilterModel? filter)
        {
            var result = _recipeRepository.GetAllRecipes().Where(r => r.Active == true);
            if (!string.IsNullOrWhiteSpace(filter.Ingredients))
            {
                result = result.Where(r => r.Ingredients.Contains(filter.Ingredients));
            }

            if (filter.CategoryId != null)
            {
                result = result.Where(r => r.CategoryId == filter.CategoryId);
            }

            if (filter.CookingTimeInMins != null)
            {
                result = result.Where(r => r.CookingTimeInMins <= filter.CookingTimeInMins);
            }


            return result;
        }

        // GET: api/Recipe/5
        [HttpGet("{id}")]
        public ActionResult<RecipeModel> GetRecipe(int id)
        {
            var recipe = _recipeRepository.GetRecipeById(id);

            if (recipe == null)
            {
                return NotFound();
            }
            else
            {
                return recipe;
            }
        }

        // PUT: api/Recipe/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<RecipeModel>> PutRecipe(int id, RecipeModel recipeModel)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var IsAdmin = User.IsInRole("Admin");

            var recipe = _recipeRepository.GetRecipeById(id);

            if (recipe == null)
            {
                return NotFound();
            }

            if (IsAdmin || recipe.AuthorId == userId)
            {
                _recipeRepository.UpdateRecipe(recipeModel);
                return recipeModel;
            }
            else
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }
        }

        // POST: api/Recipe
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Recipe>> PostRecipe(RecipeModel recipeModel)
        {
            recipeModel.AuthorId = int.Parse(User.FindFirst("UserId")?.Value);

            var result = _recipeRepository.AddRecipe(recipeModel);

            return CreatedAtAction("GetRecipe", new { id = result.Id }, recipeModel);
        }

        // DELETE: api/Recipe/5
        [HttpDelete("{id}")]
        public ActionResult<BaseResponse> DeleteRecipe(int id)
        {
            var response = new BaseResponse();
            var recipe = _recipeRepository.GetRecipeById(id);

            if (recipe == null)
            {
                response.Status = Status.Error;
                response.Message = "Recipe not found.";
            }

            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var IsAdmin = User.IsInRole("Admin");
            if (IsAdmin || recipe.AuthorId == userId)
            {
                response.Status = Status.Success;
                response.Message = "User deleted successfully.";

                return response;
            }
            else
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }
        }
    }
}
