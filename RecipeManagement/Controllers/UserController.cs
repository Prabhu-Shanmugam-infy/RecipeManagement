using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeManagement.Contracts;
using RecipeManagement.Entities;
using RecipeManagement.Models;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RecipeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration _config;
        private readonly RecipeContext _context;
        private readonly IMapper _mapper;

        public UserController(IConfiguration config, RecipeContext context, IMapper mapper)
        {
            _config = config;
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IEnumerable<UserModel>> GetAll()
        {
            var users = _context.Users.ToList(); ;
            IEnumerable<UserModel> ilistDest = _mapper.Map<IEnumerable<User>, IEnumerable<UserModel>>(users);
            return ilistDest;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            var userModel = _mapper.Map<User, UserModel>(user);
            return userModel;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{userid}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<UserModel>> PutUser(int userid, UserModel userModel)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var IsAdmin = User.IsInRole("Admin");

            var user = _context.Users.Find(userModel.Id);
            if (IsAdmin || user.Id == userId)
            {
                var oldPassword = user.PasswordHash;
                _mapper.Map<UserModel, User>(userModel, user);

                if (!IsAdmin)
                {
                    user.IsAdmin = 0;
                }

                user.PasswordHash = oldPassword;
                _context.Entry(user).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
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
        public async Task<ActionResult<UserModel>> PostUser(UserModel userModel)
        {
            var user = _mapper.Map<UserModel, User>(userModel);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            userModel.Id = user.Id;

            return userModel;
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResponse>> DeleteUser(int id)
        {
            var response = new BaseResponse();
            var UserEntity = await _context.Users.FindAsync(id);


            if (UserEntity == null)
            {
                response.Status = Status.Error;
                response.Message = "User not found.";
                return NotFound();
            }

            _context.Users.Remove(UserEntity);
            await _context.SaveChangesAsync();

            response.Status = Status.Success;
            response.Message = "User deleted successfully.";

            return response;

        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
