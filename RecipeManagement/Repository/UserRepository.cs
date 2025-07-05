using AutoMapper;
using NuGet.Packaging.Signing;
using RecipeManagement.Entities;
using RecipeManagement.Interface;
using RecipeManagement.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RecipeManagement.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly RecipeContext _context;
        private readonly IMapper _mapper;

        public UserRepository(RecipeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



        public UserModel AddUser(UserModel userModel)
        {
            var user = _mapper.Map<UserModel, User>(userModel);
            user.PasswordHash = MD5Helper.GetMd5Hash(userModel.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
            userModel.Id = user.Id;
            return userModel;
        }


        public bool DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                var recipes = _context.Recipes.Where(r => r.AuthorId == id).ToList();
                var recipeImages = _context.RecipeImages.Where(ri => recipes.Select(r=>r.Id).Contains(ri.RecipeId)).ToList();
                
                _context.RecipeImages.RemoveRange(recipeImages);
                _context.Recipes.RemoveRange(recipes);
                _context.Users.Remove(user);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<UserModel> GetAllUsers()
        {
            var users = _context.Users.ToList(); ;
            List<UserModel> ilistDest = _mapper.Map<List<User>, List<UserModel>>(users);
            return ilistDest;
        }

        public UserModel GetUserById(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                return _mapper.Map<User, UserModel>(user);
            }
            else
            {
                return null;
            }
        }

        public bool UpdateUser(UserModel userModel)
        {
            var user = _context.Users.Find(userModel.Id);
            if (user != null)
            {
                user.ProfilePicture = userModel.ProfilePicture;
                user.IsActive = userModel.IsActive ? 1 : 0;
                user.IsAdmin = userModel.IsAdmin ? 1 : 0;
                user.Bio = userModel.Bio;
                user.Name = userModel.UserName;
                user.Email = userModel.Email;
                _context.Users.Update(user);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public UserModel ValidateLogin(string emailId, string password)
        {
            var user = _context.Users
                .Where(u => u.IsActive == 1 && u.Email == emailId && u.PasswordHash == MD5Helper.GetMd5Hash(password))
                .FirstOrDefault();
            if (user != null)
            {
                return _mapper.Map<UserModel>(user);
            }
            else
            {
                return null;
            }

        }
    }
}
