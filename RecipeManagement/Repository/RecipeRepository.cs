using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipeManagement.Entities;
using RecipeManagement.Interface;
using RecipeManagement.Models;

namespace RecipeManagement.Repository
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly RecipeContext _context;
        private readonly IMapper _mapper;

        public RecipeRepository(RecipeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public RecipeModel AddRecipe(RecipeModel recipeModel)
        {
            var recipe = new Recipe();
            recipe.Active = 1;
            recipe.Title = recipeModel.Title;
            recipe.AuthorId = recipeModel.AuthorId.Value;
            recipe.CookingTimeInMins = recipeModel.CookingTimeInMins;
            recipe.CategoryId = recipeModel.CategoryId;
            recipe.Ingredients = recipeModel.Ingredients;
            recipe.Instructions = recipeModel.Instructions;
            recipe.RecipeImages = recipeModel.RecipeImages.Select(i => new RecipeImage() { Recipe = recipe, Path = i }).ToList();
            _context.Recipes.Add(recipe);

            _context.SaveChanges();

            recipeModel.Id = recipe.Id;

            return recipeModel;
        }

        public bool DeleteRecipe(int id)
        {
            var recipe = _context.Recipes
               .Include(r => r.RecipeImages)
               .Include(r => r.Author)
               .Include(r => r.Category)
               .Where(r => r.Id == id).FirstOrDefault();

            if (recipe != null)
            {
                foreach (var img in recipe.RecipeImages)
                {
                    //_context.Entry(img).State = EntityState.Deleted;
                    _context.RecipeImages.Remove(img);
                }
                _context.Recipes.Remove(recipe);

                _context.SaveChanges();
            }

            return true;
        }

        public List<RecipeModel> GetAllRecipes()
        {
            var lst = _context.Recipes
               .Include(r => r.RecipeImages)
               .Include(r => r.Category)
                   .Include(r => r.Author).ToList();
            List<RecipeModel> ilistDest = _mapper.Map<List<Recipe>, List<RecipeModel>>(lst);
            return ilistDest;
        }

        public RecipeModel GetRecipeById(int id)
        {
            var recipe = _context.Recipes
                .Include(r => r.RecipeImages)
                .Include(r => r.Author)
                .Include(r => r.Category)
                .Where(r => r.Id == id).FirstOrDefault();

            if (recipe == null)
            {
                return null;
            }
            else
            {
                var recipeModel = _mapper.Map<Recipe, RecipeModel>(recipe);
                return recipeModel;
            }
        }

        public bool UpdateRecipe(RecipeModel recipeModel)
        {
            var recipe = _context.Recipes
               .Include(r => r.RecipeImages)
                   .Include(r => r.Author)
                   .Include(r => r.Category)
               .Where(r => r.Id == recipeModel.Id).FirstOrDefault();

            recipe.Title = recipeModel.Title;
            recipe.CookingTimeInMins = recipeModel.CookingTimeInMins;
            recipe.CategoryId = recipeModel.CategoryId;
            recipe.Ingredients = recipeModel.Ingredients;
            recipe.Instructions = recipeModel.Instructions;
            recipe.Active = recipeModel.Active ? 1 : 0;

            //Remove Deleted
            var toremove = recipe.RecipeImages.Where(i => !recipeModel.RecipeImages.Contains(i.Path)).ToList();
            var alreadyExisting = recipe.RecipeImages.Where(i => recipeModel.RecipeImages.Contains(i.Path)).Select(i => i.Path).ToList();
            var toadd = recipeModel.RecipeImages.Except(alreadyExisting);
            foreach (var img in toremove)
            {
                //_context.Entry(img).State = EntityState.Deleted;
                recipe.RecipeImages.Remove(img);

            }

            foreach (var img in toadd)
            {
                var recipeimage = new RecipeImage() { Recipe = recipe, Path = img };
                recipe.RecipeImages.Add(recipeimage);
            }

            //_context.Entry(recipe).State = EntityState.Modified;
            _context.Recipes.Update(recipe);
            _context.SaveChanges();

            return true;
        }
    }
}
