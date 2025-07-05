using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipeManagement.Entities;
using RecipeManagement.Interface;
using RecipeManagement.Models;

namespace RecipeManagement.Repository
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly RecipeContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(RecipeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<CategoryModel> GetAllCategories()
        {
            return _context.Categories.Select(c => new CategoryModel() {
                Id = c.Id, 
                Name = c.Name 
            }).ToList();
        }


    }
}
