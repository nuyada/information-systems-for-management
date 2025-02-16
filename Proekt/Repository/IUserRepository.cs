using Proekt.Entites;
using Proekt.Models;

namespace Proekt.Repository
{
    public interface IUserRepository
    {
        IEnumerable<Users> GetAllUsers();
        Users GetUserById(int Id);
        void CreateUser(Users users);
        void UpdateUser(UserModel users,int Id);
        void DeleteUser(int Id);
        Users GetUserByEmail(string email);
        public List<string> GetUserRoles(int userId);
        public void AssingRoleToUser(int userId, int roleId);

    }
}
