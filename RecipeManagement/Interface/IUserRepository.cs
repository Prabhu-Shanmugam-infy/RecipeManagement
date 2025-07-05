using RecipeManagement.Models;

namespace RecipeManagement.Interface
{
    public interface IUserRepository
    {
        UserModel ValidateLogin(string emailId, string password);

        List<UserModel> GetAllUsers();

        UserModel GetUserById(int id);

        UserModel AddUser(UserModel userModel);

        bool DeleteUser(int id);

        bool UpdateUser(UserModel userModel);

    }
}
