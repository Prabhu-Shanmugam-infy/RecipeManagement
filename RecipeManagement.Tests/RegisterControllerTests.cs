using global::RecipeManagement.Contracts;
using global::RecipeManagement.Controllers;
using global::RecipeManagement.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using RecipeManagement.Contracts;
using RecipeManagement.Controllers;
using RecipeManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RecipeManagement.Tests
{

    public class RegisterControllerTests
    {
        private Mock<IConfiguration> _configMock;
        private Mock<RecipeContext> _contextMock;
        private Mock<DbSet<User>> _userDbSetMock;
        private List<User> _userList;
        private RegisterController _controller;

        [SetUp]
        public void Setup()
        {
            _configMock = new Mock<IConfiguration>();

            _userList = new List<User>();
            var queryable = _userList.AsQueryable();

            _userDbSetMock = new Mock<DbSet<User>>();
            _userDbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(queryable.Provider);
            _userDbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(queryable.Expression);
            _userDbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            _userDbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            _userDbSetMock.Setup(d => d.Add(It.IsAny<User>())).Callback<User>(u =>
            {
                u.Id = 101; // simulate auto-increment
                _userList.Add(u);
            });

            _contextMock = new Mock<RecipeContext>();
            _contextMock.Setup(c => c.Users).Returns(_userDbSetMock.Object);
            _contextMock.Setup(c => c.SaveChanges()).Returns(1);

            _controller = new RegisterController(_configMock.Object, _contextMock.Object);
        }

        [Test]
        public void Register_ValidRequest_ReturnsSuccess()
        {
            // Arrange
            var request = new RegisterRequest
            {
                UserName = "JohnDoe",
                Email = "john@example.com",
                Password = "password123"
            };

            // Act
            var result = _controller.Register(request);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(Status.Success, result.Value.Status);
            Assert.AreEqual("response", result.Value.Message);
            Assert.AreEqual("101", result.Value.UserId);
        }

        [Test]
        public void Register_EmptyUserName_ReturnsError()
        {
            var request = new RegisterRequest
            {
                UserName = "",
                Email = "test@example.com",
                Password = "password"
            };

            var result = _controller.Register(request);

            Assert.AreEqual(Status.Error, result.Value.Status);
            Assert.AreEqual("Username is mandatory.", result.Value.Message);
        }

        [Test]
        public void Register_InvalidEmail_ReturnsError()
        {
            var request = new RegisterRequest
            {
                UserName = "TestUser",
                Email = "invalid-email",
                Password = "password"
            };

            var result = _controller.Register(request);

            Assert.AreEqual(Status.Error, result.Value.Status);
            Assert.AreEqual("Email is not valid.", result.Value.Message);
        }

        [Test]
        public void Register_DuplicateEmail_ReturnsError()
        {
            // Simulate existing user
            _userList.Add(new User { Id = 1, Email = "test@example.com" });

            var request = new RegisterRequest
            {
                UserName = "TestUser",
                Email = "test@example.com",
                Password = "password"
            };

            var result = _controller.Register(request);

            Assert.AreEqual(Status.Error, result.Value.Status);
            Assert.AreEqual("Email is already exists.", result.Value.Message);
        }
    }
}

