using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using RecipeManagement.Controllers;
using RecipeManagement.Interface;
using RecipeManagement.Models;
using RecipeManagement.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace RecipeManagement.Tests
{
    public class LoginControllerTests
    {
        private Mock<IConfiguration> _configMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IUserRepository> _userRepoMock;
        private LoginController _controller;

        [SetUp]
        public void Setup()
        {
            _configMock = new Mock<IConfiguration>();
            _mapperMock = new Mock<IMapper>();
            _userRepoMock = new Mock<IUserRepository>();

            // Setup JWT config values
            _configMock.Setup(c => c["Jwt:Key"]).Returns("FrK7GXnBV9pmlK2S0VM8BlkcmIOCuTsO");
            _configMock.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");

            _controller = new LoginController(
                _configMock.Object,
                _mapperMock.Object,
                _userRepoMock.Object
            );
        }

        [Test]
        public void Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "user@example.com",
                Password = "password"
            };

            var user = new UserModel
            {
                Id = 1,
                UserName = "TestUser",
                Email = "user@example.com",
                IsAdmin = false
            };

            _userRepoMock.Setup(repo => repo.ValidateLogin(loginRequest.Email, loginRequest.Password))
                         .Returns(user);

            // Act
            var result = _controller.Login(loginRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult?.Value);
            var response = okResult.Value as LoginResponse;
            Assert.IsNotNull(response?.Token);
            Assert.IsTrue(response.Token.Length > 10); // basic check
        }

        [Test]
        public void Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "wrong@example.com",
                Password = "wrongpassword"
            };

            _userRepoMock.Setup(repo => repo.ValidateLogin(loginRequest.Email, loginRequest.Password))
                         .Returns((UserModel?)null);

            // Act
            var result = _controller.Login(loginRequest);

            // Assert
            Assert.IsInstanceOf<UnauthorizedResult>(result);
        }
    }
}

