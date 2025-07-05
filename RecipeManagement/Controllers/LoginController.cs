using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RecipeManagement.Contracts;
using RecipeManagement.Interface;
using RecipeManagement.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecipeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;        
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public LoginController(IConfiguration config,  IMapper mapper, IUserRepository userRepository)
        {
            _config = config;            
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] Contracts.LoginRequest data)
        {
            var user = _userRepository.ValidateLogin(data.Email, data.Password);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                return Ok(new LoginResponse() { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
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
    }
}
