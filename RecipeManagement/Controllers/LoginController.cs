using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RecipeManagement.Contracts;
using RecipeManagement.Entities;
using RecipeManagement.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RecipeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly RecipeContext _context;
        private readonly IMapper _mapper;

        public LoginController(IConfiguration config, RecipeContext context, IMapper mapper)
        {
            _config = config;
            _context = context;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] Contracts.LoginRequest data)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(data);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new LoginResponse() { Token = tokenString });
            }

            return response;
        }
        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim("UserId", userInfo.Id.ToString()),                
                 new Claim(ClaimTypes.Role, userInfo.IsAdmin ? "Admin" : "User"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel AuthenticateUser(Contracts.LoginRequest data)
        {
            var user = _context.Users.Where(u => u.IsActive == 1 &&  u.Email == data.Email && u.PasswordHash == MD5Helper.GetMd5Hash(data.Password)).FirstOrDefault();

            if (user != null)
            {
                var userModel = _mapper.Map<UserModel>(user);
                return userModel;
            }
            else { return null; }


        }
    }
}
