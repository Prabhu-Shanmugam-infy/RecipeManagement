using RecipeManagement.Models;

namespace RecipeManagement.Interface
{
    public interface  ICategoryRepository
    {
        List<CategoryModel> GetAllCategories();
    }
}
