using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RecipeManagement.Contracts;
using RecipeManagement.Entities;
using System;

namespace RecipeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private IConfiguration _config;
        private readonly RecipeContext _context;
        public RegisterController(IConfiguration config, RecipeContext context)
        {
            _config = config;
            _context = context;
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult<RegisterResponse> Register([FromBody] Contracts.RegisterRequest data)
        {
            var response = new RegisterResponse();
            if (IsValidRequest(data, response))
            {
                var user = new User();
                user.IsActive = 1;
                user.Name = data.UserName;
                user.Email = data.Email;
                user.PasswordHash = MD5Helper.GetMd5Hash(data.Password);
                _context.Users.Add(user);
                _context.SaveChanges();
                
                response.UserId = user.Id.ToString();
                response.Message = "response";
                response.Status = Status.Success;

            }
            return response;

        }

        private bool IsValidRequest(Contracts.RegisterRequest data, RegisterResponse response)
        {
            response.Status = Status.Error;
            if (string.IsNullOrEmpty(data.UserName))
            {
                response.Message = "Username is mandatory.";
                return false;
            }
            else if (string.IsNullOrEmpty(data.Email))
            {
                response.Message = "Email is mandatory.";
                return false;
            }
            else if (!ValidationHelper.IsValidEmail(data.Email))
            {
                response.Message = "Email is not valid.";
                return false;
            }

            var cnt = _context.Users.Where(u => u.Email.ToLower() == data.Email.ToLower()).Count();
            if (cnt > 0)
            {
                response.Message = "Email is already exists.";
                return false;
            }

            return true;

        }
    }
}
