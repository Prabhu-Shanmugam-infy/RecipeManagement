using AutoMapper;
using global::RecipeManagement.Contracts;
using global::RecipeManagement.Controllers;
using global::RecipeManagement.Interface;
using global::RecipeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Net;
using System.Security.Claims;

namespace RecipeManagement.Tests
{

    [TestFixture]
    public class RecipeControllerTests
    {
        private Mock<IConfiguration> _configMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IRecipeRepository> _recipeRepoMock;
        private RecipeController _controller;

        [SetUp]
        public void Setup()
        {
            _configMock = new Mock<IConfiguration>();
            _mapperMock = new Mock<IMapper>();
            _recipeRepoMock = new Mock<IRecipeRepository>();

            _controller = new RecipeController(
                _configMock.Object,
                _mapperMock.Object,
                _recipeRepoMock.Object
            );
        }

        private void SetUserContext(int userId, string role)
        {
            var claims = new List<Claim>
            {
                new Claim("UserId", userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var user = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Test]
        public void GetRecipesAsync_ReturnsListOfRecipes()
        {
            var recipes = new List<RecipeModel> { new RecipeModel { Id = 1 }, new RecipeModel { Id = 2 } };
            _recipeRepoMock.Setup(r => r.GetAllRecipes()).Returns(recipes);

            var result = _controller.GetRecipesAsync();

            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task SearchRecipesAsync_WithQuery_ReturnsFilteredResults()
        {
            var recipes = new List<RecipeModel>
            {
                new RecipeModel { Id = 1, Title = "Pasta", Ingredients = "Tomato", Instructions = "Cook", Active = true },
                new RecipeModel { Id = 2, Title = "Burger", Ingredients = "Meat", Instructions = "Grill", Active = true }
            };

            _recipeRepoMock.Setup(r => r.GetAllRecipes()).Returns(recipes);

            var result = await _controller.SearchRecipesAsync("pasta");

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public async Task FilterRecipesAsync_WithFilter_ReturnsFilteredResults()
        {
            var recipes = new List<RecipeModel>
            {
                new RecipeModel { Id = 1, Ingredients = "Cheese", CategoryId = 1, CookingTimeInMins = 10, Active = true },
                new RecipeModel { Id = 2, Ingredients = "Meat", CategoryId = 2, CookingTimeInMins = 20, Active = true }
            };
            _recipeRepoMock.Setup(r => r.GetAllRecipes()).Returns(recipes);

            var filter = new FilterModel { Ingredients = "Cheese", CategoryId = 1, CookingTimeInMins = 15 };

            var result = await _controller.FilerRecipesAsync(filter);

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void GetRecipe_ValidId_ReturnsRecipe()
        {
            var recipe = new RecipeModel { Id = 5 };
            _recipeRepoMock.Setup(r => r.GetRecipeById(5)).Returns(recipe);

            var result = _controller.GetRecipe(5);

            Assert.IsInstanceOf<RecipeModel>(result.Value);
            Assert.AreEqual(5, result.Value.Id);
        }

        [Test]
        public void GetRecipe_InvalidId_ReturnsNotFound()
        {
            _recipeRepoMock.Setup(r => r.GetRecipeById(99)).Returns((RecipeModel)null);

            var result = _controller.GetRecipe(99);

            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task PutRecipe_AuthorizedUser_UpdatesAndReturnsRecipe()
        {
            var recipe = new RecipeModel { Id = 3, AuthorId = 10 };
            _recipeRepoMock.Setup(r => r.GetRecipeById(3)).Returns(recipe);

            SetUserContext(10, "User");

            var updated = new RecipeModel { Id = 3, AuthorId = 10, Title = "Updated" };

            var result = await _controller.PutRecipe(3, updated);

            Assert.IsInstanceOf<RecipeModel>(result.Value);
            Assert.AreEqual("Updated", result.Value.Title);
        }

        [Test]
        public async Task PutRecipe_UnauthorizedUser_ReturnsForbidden()
        {
            var recipe = new RecipeModel { Id = 4, AuthorId = 10 };
            _recipeRepoMock.Setup(r => r.GetRecipeById(4)).Returns(recipe);

            SetUserContext(11, "User");

            var result = await _controller.PutRecipe(4, new RecipeModel { Id = 4 });

            //Assert.IsInstanceOf<ObjectResult>(result.Result);
            //var statusCode = (ObjectResult)result.Result;
            //Assert.AreEqual((int)HttpStatusCode.Forbidden, statusCode.StatusCode);

            Assert.IsInstanceOf<ActionResult<RecipeModel>>(result);
            var res = (ActionResult<RecipeModel>?)result;
            var status = (StatusCodeResult)res.Result;

            Assert.AreEqual((int)HttpStatusCode.Forbidden, status.StatusCode);
        }

        [Test]
        public async Task PostRecipe_ValidRequest_ReturnsCreated()
        {
            SetUserContext(9, "User");

            var model = new RecipeModel { Title = "New Recipe" };
            var recipe = new RecipeModel { Id = 22 };

            _recipeRepoMock.Setup(r => r.AddRecipe(It.IsAny<RecipeModel>())).Returns(recipe);

            var result = await _controller.PostRecipe(model);

            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var created = result.Result as CreatedAtActionResult;
            Assert.AreEqual("GetRecipe", created.ActionName);
        }

        [Test]
        public void DeleteRecipe_AdminUser_DeletesSuccessfully()
        {
            var recipe = new RecipeModel { Id = 1, AuthorId = 5 };
            _recipeRepoMock.Setup(r => r.GetRecipeById(1)).Returns(recipe);

            SetUserContext(2, "Admin");

            var result = _controller.DeleteRecipe(1);

            var response = result.Value as BaseResponse;
            Assert.AreEqual(Status.Success, response.Status);
        }

        [Test]
        public void DeleteRecipe_UnauthorizedUser_ReturnsForbidden()
        {
            var recipe = new RecipeModel { Id = 1, AuthorId = 100 };
            _recipeRepoMock.Setup(r => r.GetRecipeById(1)).Returns(recipe);

            SetUserContext(10, "User");

            var result = _controller.DeleteRecipe(1);

            Assert.IsInstanceOf<ActionResult<BaseResponse>>(result);
            var res = (ActionResult<BaseResponse>?)result;
            var status = (StatusCodeResult)res.Result;

            Assert.AreEqual((int)HttpStatusCode.Forbidden, status.StatusCode);
        }
    }
}


