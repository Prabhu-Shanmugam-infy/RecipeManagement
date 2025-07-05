using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeManagement.Contracts;
using RecipeManagement.Entities;
using RecipeManagement.Interface;
using RecipeManagement.Models;
using RecipeManagement.Repository;
using System.Net;

namespace RecipeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration _config;
        private readonly RecipeContext _context;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserController(IConfiguration config, RecipeContext context, IMapper mapper, IUserRepository userRepository)
        {
            _config = config;
            _context = context;
            _mapper = mapper;
            _userRepository = userRepository;
        }


        [HttpGet]
        public async Task<IEnumerable<UserModel>> GetAll()
        {
            return _userRepository.GetAllUsers();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUser(int id)
        {
            var user = _userRepository.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return user;
            }
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{userid}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<UserModel>> PutUser(int userid, UserModel userModel)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var IsAdmin = User.IsInRole("Admin");

            var user = _userRepository.GetUserById(userid);

            if (user == null)
            {
                return NotFound();
            }

            if (IsAdmin || user.Id == userId)
            {
                if (!IsAdmin)
                {
                    userModel.IsAdmin = false;
                }

                _userRepository.UpdateUser(userModel);

                return userModel;
            }
            else
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<UserModel> PostUser(UserModel userModel)
        {
            return _userRepository.AddUser(userModel);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<BaseResponse> DeleteUser(int id)
        {
            var response = new BaseResponse();

            var result = _userRepository.DeleteUser(id);
            if (result)
            {
                response.Status = Status.Success;
                response.Message = "User deleted successfully.";
            }
            else
            {
                response.Status = Status.Error;
                response.Message = "User not found.";
                //return NotFound();
            }

            return response;
        }
    }
}
