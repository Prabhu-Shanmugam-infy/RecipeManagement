using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Elfie.Serialization;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeManagement.Contracts;
using RecipeManagement.Entities;
using RecipeManagement.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RecipeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private IConfiguration _config;
        private readonly RecipeContext _context;
        private readonly IMapper _mapper;

        public RecipeController(IConfiguration config, RecipeContext context, IMapper mapper)
        {
            _config = config;
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Recipes
        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<IEnumerable<RecipeModel>> GetRecipesAsync()
        {
           
            var lst = await _context.Recipes
                .Include(r => r.RecipeImages)
                .Include(r => r.Category)
                    .Include(r => r.Author).ToListAsync();
            IEnumerable<RecipeModel> ilistDest = _mapper.Map<IEnumerable<Recipe>, IEnumerable<RecipeModel>>(lst);
            return ilistDest;
        }

        [HttpGet("search/{query?}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IEnumerable<RecipeModel>> SearchRecipesAsync(string? query)
        {
            var lst = new List<Recipe>();
            if (string.IsNullOrWhiteSpace(query))
            {
                lst = await _context.Recipes
                    .Include(r => r.RecipeImages)
                    .Include(r => r.Author)
                    .Include(r => r.Category)
                    .Where(r => r.Active == 1).ToListAsync();
            }
            else
            {
                lst = await _context.Recipes
                     .Include(r => r.RecipeImages)
                     .Include(r => r.Author)
                     .Include(r => r.Category)
                    .Where(r => r.Active == 1 && (r.Ingredients.ToLower().Contains(query.ToLower())
              || r.Title.ToLower().Contains(query.ToLower())
              || r.Instructions.ToLower().Contains(query.ToLower()))).ToListAsync();
            }

            IEnumerable<RecipeModel> ilistDest = _mapper.Map<IEnumerable<Recipe>, IEnumerable<RecipeModel>>(lst);
            return ilistDest;
        }

        [HttpPost("filter")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IEnumerable<RecipeModel>> FilerRecipesAsync(FilterModel? filter)
        {
            IQueryable<Recipe> lst = _context.Recipes
                    .Include(r => r.RecipeImages)
                    .Include(r => r.Author)
                    .Include(r => r.Category);
            if (!string.IsNullOrWhiteSpace(filter.Ingredients))
            {
                lst = lst.Where(r => r.Active == 1 && r.Ingredients.Contains(filter.Ingredients));
            }
            if (filter.CategoryId != null)
            {
                lst = lst.Where(r => r.CategoryId == filter.CategoryId);
            }

            if (filter.CookingTimeInMins != null)
            {
                lst = lst.Where(r => r.CookingTimeInMins <= filter.CookingTimeInMins);
            }
            var lstResult = await lst.ToListAsync();
            IEnumerable<RecipeModel> ilistDest = _mapper.Map<IEnumerable<Recipe>, IEnumerable<RecipeModel>>(lstResult);
            return ilistDest;
        }

        // GET: api/Recipe/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeModel>> GetRecipe(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.RecipeImages)
                   .Include(r => r.Author)
                   .Include(r => r.Category).Where(r => r.Id == id).FirstOrDefaultAsync();

            if (recipe == null)
            {
                return NotFound();
            }
            var recipeModel = _mapper.Map<Recipe, RecipeModel>(recipe);
            return recipeModel;
        }

        // PUT: api/Recipe/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<RecipeModel>> PutRecipe(int id, RecipeModel recipeModel)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var IsAdmin = User.IsInRole("Admin");

            var recipe = _context.Recipes
                .Include(r => r.RecipeImages)
                    .Include(r => r.Author)
                    .Include(r => r.Category)
                .Where(r => r.Id == recipeModel.Id).FirstOrDefault();

            if (IsAdmin || recipe.AuthorId == userId)
            {

                recipe.Title = recipeModel.Title;
                recipe.CookingTimeInMins = recipeModel.CookingTimeInMins;
                recipe.CategoryId = recipeModel.CategoryId;
                recipe.Ingredients = recipeModel.Ingredients;
                recipe.Instructions = recipeModel.Instructions;

                //Remove Deleted
                var toremove = recipe.RecipeImages.Where(i => !recipeModel.RecipeImages.Contains(i.Path)).ToList();
                var alreadyExisting = recipe.RecipeImages.Where(i => recipeModel.RecipeImages.Contains(i.Path)).Select(i => i.Path).ToList();
                var toadd = recipeModel.RecipeImages.Except(alreadyExisting);
                foreach (var img in toremove)
                {
                    _context.Entry(img).State = EntityState.Deleted;
                    recipe.RecipeImages.Remove(img);

                }

                foreach (var img in toadd)
                {
                    var recipeimage = new RecipeImage() { Recipe = recipe, Path = img };
                    recipe.RecipeImages.Add(recipeimage);
                }

                _context.Entry(recipe).State = EntityState.Modified;
                await _context.SaveChangesAsync();

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
            var recipe = new Recipe();
            recipe.Active = 1;
            recipe.Title = recipeModel.Title;
            recipe.AuthorId = int.Parse(User.FindFirst("UserId")?.Value);
            recipe.CookingTimeInMins = recipeModel.CookingTimeInMins;
            recipe.CategoryId = recipeModel.CategoryId;
            recipe.Ingredients = recipeModel.Ingredients;
            recipe.Instructions = recipeModel.Instructions;
            recipe.RecipeImages = recipeModel.RecipeImages.Select(i => new RecipeImage() { Recipe = recipe, Path = i }).ToList();
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipe", new { id = recipe.Id }, recipeModel);
        }

        // DELETE: api/Recipe/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse>> DeleteRecipe(int id)
        {
            var response = new BaseResponse();
            var recipe = _context.Recipes
                .Include(r => r.RecipeImages)
                    .Include(r => r.Author).Where(r => r.Id == id).FirstOrDefault();
            
            if (recipe == null)
            {
                response.Status = Status.Error;
                response.Message = "Recipe not found.";
            }

            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var IsAdmin = User.IsInRole("Admin");
            if (IsAdmin || recipe.AuthorId == userId)
            {
                foreach (var img in recipe.RecipeImages)
                {
                    _context.Entry(img).State = EntityState.Deleted;
                }

                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();

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
