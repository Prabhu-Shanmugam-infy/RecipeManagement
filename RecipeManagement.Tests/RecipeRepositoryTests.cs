using RecipeManagement.Entities;
using RecipeManagement.Interface;
using RecipeManagement.Models;
using RecipeManagement.Tests.Mock;

namespace RecipeManagement.Tests
{
    public class RecipeRepositoryTests
    {
        private IRecipeRepository recipeRepository;
        [SetUp]
        public void Setup()
        {
            recipeRepository = IRecipeRepositoryMock.GetMock();
        }

        [Test]
        public void GetRecipes()
        {
            //Arrange

            //Act
            IList<RecipeModel> lstData = recipeRepository.GetAllRecipes();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(lstData, Is.Not.Null);
                Assert.That(lstData.Count, Is.GreaterThan(0));
            });
        }

        [Test]
        public void GetRecipeById()
        {
            //Arrange
            int id = 1;

            //Act
            RecipeModel data = recipeRepository.GetRecipeById(id);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(data, Is.Not.Null);
                Assert.That(data.Id, Is.EqualTo(id));
            });
        }

        [Test]
        public void AddRecipe()
        {
            //Arrange
            RecipeModel recipeModel = new RecipeModel()
            {
                Id = 100,
                Title = "recipe1",
                Ingredients = "",
                Instructions = "",
                Active = true,
                AuthorId = 1,
                Author = new UserModel() { Id = 1, UserName = "admin" },
                CategoryId = 1,
                Category = new CategoryModel() { Id = 1, Name = "Category" },
                CookingTimeInMins = 20,
                RecipeImages = new List<String>()
            };

            //Act
            var data = recipeRepository.AddRecipe(recipeModel);
            var expectedData = recipeRepository.GetRecipeById(recipeModel.Id);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(data, Is.Not.Null);
                Assert.That(expectedData, Is.Not.Null);
                Assert.That(expectedData.Id, Is.EqualTo(recipeModel.Id));
            });
        }

        [Test]
        public void UpdateRecipe()
        {
            //Arrange
            int id = 1;
            var actualData = recipeRepository.GetRecipeById(id);
            actualData.Title = "Recipe Edited 1";           

            //Act
            bool data = recipeRepository.UpdateRecipe(actualData);
            var expectedData = recipeRepository.GetRecipeById(actualData.Id);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(data, Is.True);
                Assert.That(expectedData, Is.Not.Null);
                Assert.That(expectedData.Title , Is.EqualTo(actualData.Title));
            });
        }

        [Test]
        public void DeleteRecipe()
        {
            //Arrange
            int id = 1;
            var actualData = recipeRepository.GetRecipeById(id);

            
            //Act
            bool data = recipeRepository.DeleteRecipe(actualData.Id);
            var  expectedData = recipeRepository.GetRecipeById(actualData.Id);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(data, Is.True);
                Assert.That(expectedData, Is.Null);
            });
        }
    }


}