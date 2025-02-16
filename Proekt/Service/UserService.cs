namespace Proekt.Service
{
    using Proekt.Entites;
    using Proekt.Models;
    using Proekt.Repository;
    using System.Collections.Generic;

    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<Users> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public Users GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public void CreateUser(Users user)
        {
            var existingUser = _userRepository.GetUserByEmail(user.Email);
            if (existingUser != null)
            {
                throw new Exception("Пользователь с таким Email уже существует.");
            }

            _userRepository.CreateUser(user);
        }

        public void UpdateUser(UserModel user,int Id)
        {
            var existingUser = _userRepository.GetUserById(Id);
            if (existingUser == null)
            {
                throw new Exception("Пользователь не найден.");
            }

            _userRepository.UpdateUser(user,Id);
        }

        public void DeleteUser(int id)
        {
            var existingUser = _userRepository.GetUserById(id);
            if (existingUser == null)
            {
                throw new Exception("Пользователь не найден.");
            }

            _userRepository.DeleteUser(id);
        }
    }

}
