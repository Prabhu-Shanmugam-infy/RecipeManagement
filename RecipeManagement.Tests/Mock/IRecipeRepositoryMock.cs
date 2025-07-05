using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using RecipeManagement.Entities;
using RecipeManagement.Interface;
using RecipeManagement.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManagement.Tests.Mock
{
    public class IRecipeRepositoryMock
    {
        public static IRecipeRepository GetMock()
        {
            List<Recipe> lstRecipe = GenerateTestData();
            //RecipeContext dbContextMock = DbContextMock.GetMock<Recipe, RecipeContext>(lstRecipe, x => x.Recipes);
            IQueryable<Recipe> lstDataQueryable = lstRecipe.AsQueryable();
            Mock<DbSet<Recipe>> dbSetMock = new Mock<DbSet<Recipe>>();
            Mock<RecipeContext> dbContext = new Mock<RecipeContext>();


            dbSetMock.As<IQueryable<Recipe>>().Setup(s => s.Provider).Returns(lstDataQueryable.Provider);
            dbSetMock.As<IQueryable<Recipe>>().Setup(s => s.Expression).Returns(lstDataQueryable.Expression);
            dbSetMock.As<IQueryable<Recipe>>().Setup(s => s.ElementType).Returns(lstDataQueryable.ElementType);

            dbSetMock.As<IQueryable<Recipe>>().Setup(s => s.GetEnumerator()).Returns(() => lstDataQueryable.GetEnumerator());
            dbSetMock.Setup(x => x.Add(It.IsAny<Recipe>())).Callback<Recipe>(lstRecipe.Add);
            dbSetMock.Setup(x => x.AddRange(It.IsAny<IEnumerable<Recipe>>())).Callback<IEnumerable<Recipe>>(lstRecipe.AddRange);
            dbSetMock.Setup(x => x.Remove(It.IsAny<Recipe>())).Callback<Recipe>(t => lstRecipe.Remove(t));
            dbSetMock.Setup(x => x.RemoveRange(It.IsAny<IEnumerable<Recipe>>())).Callback<IEnumerable<Recipe>>(ts =>
            {
                foreach (var t in ts) { lstRecipe.Remove(t); }
            });


            dbContext.Setup(x => x.Recipes).Returns(dbSetMock.Object);

            var listImages = new List<RecipeImage>();
            IQueryable<RecipeImage> lstDataQueryable1 = listImages.AsQueryable();

            Mock<DbSet<RecipeImage>> dbSetRIMock = new Mock<DbSet<RecipeImage>>();
            dbSetRIMock.As<IQueryable<RecipeImage>>().Setup(s => s.Provider).Returns(lstDataQueryable1.Provider);
            dbSetRIMock.As<IQueryable<RecipeImage>>().Setup(s => s.Expression).Returns(lstDataQueryable1.Expression);
            dbSetRIMock.As<IQueryable<RecipeImage>>().Setup(s => s.ElementType).Returns(lstDataQueryable1.ElementType);

            dbSetRIMock.As<IQueryable<RecipeImage>>().Setup(s => s.GetEnumerator()).Returns(() => lstDataQueryable1.GetEnumerator());

            dbContext.Setup(x => x.RecipeImages).Returns(dbSetRIMock.Object);

            RecipeContext dbContextMock =  dbContext.Object;            

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            config.AssertConfigurationIsValid();

            return new RecipeRepository(dbContextMock, config.CreateMapper());
        }

        private static List<Recipe> GenerateTestData()
        {
            var lstRecipe = new List<Recipe>();

            lstRecipe.Add(new Recipe
            {
                Id = 1,
                Title = "recipe1",
                Ingredients = "",
                Instructions = "",
                Active = 1,
                AuthorId = 1,
                Author = new User() { Id = 1, Name = "admin" },
                CategoryId = 1,
                Category = new Category() { Id = 1, Name = "Category" },
                CookingTimeInMins = 20,
                RecipeImages = new List<RecipeImage>() { new RecipeImage() { Id = 1, Path = "/", RecipeId = 1 } }

            });


            return lstRecipe;
        }
    }
}
