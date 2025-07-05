using RecipeManagement.Models;

namespace RecipeManagement.Interface
{
    public interface IRecipeRepository
    {
        List<RecipeModel> GetAllRecipes();

        RecipeModel GetRecipeById(int id);

        RecipeModel AddRecipe(RecipeModel recipeModel);

        bool DeleteRecipe(int id);

        bool UpdateRecipe(RecipeModel recipeModel);
    }   
}
