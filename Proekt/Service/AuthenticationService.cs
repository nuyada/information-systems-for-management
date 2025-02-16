using Proekt.Entites;
using Proekt.Hasher;
using Proekt.Helpers;
using Proekt.Repository;
using System.IdentityModel.Tokens.Jwt;

namespace Proekt.Service
{
    public class AuthenticationService
    {
        private readonly IUserRepository userRepository;
        private readonly JwtHelper jwtHelper;

        public AuthenticationService(IUserRepository userRepository, JwtHelper jwtHelper)
        {
            this.userRepository = userRepository;
            this.jwtHelper = jwtHelper;
            // Получаем ключ из конфигурации
        }

        public void RegisterUser(RegisterRequest registerRequest)
        {
            var existingUser = userRepository.GetUserByEmail(registerRequest.Email);
            if(existingUser != null)
            {
                throw new Exception("Пользователь с таким эмайлом уже есть");
            }
            var hashedPassword = PasswordHasher.HashPassword(registerRequest.Password);
            var user = new Users
            {
                Name = registerRequest.Name,
                MiddleName = registerRequest.MiddleName,
                LastName = registerRequest.LastName,
                PasswordHash = hashedPassword,
                Email = registerRequest.Email
            };
           
            userRepository.CreateUser(user);
            var newUser = userRepository.GetUserByEmail(registerRequest.Email);
            userRepository.AssingRoleToUser(newUser.Id, 1);

        }
        public string LoginUser(LoginRequest request)
        {
            // Проверяем, существует ли пользователь с указанным email
            var user = userRepository.GetUserByEmail(request.Email);
            if (user == null)
            {
                throw new Exception("Invalid email or password."); // Пользователь не найден
            }

            // Проверяем правильность пароля
            if (!PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new Exception("Invalid email or password."); // Неверный пароль
            }

            // Получаем роли пользователя
            var roles = userRepository.GetUserRoles(user.Id);
            if (roles == null || roles.Count == 0)
            {
                throw new Exception("User has no roles assigned."); // Пользователь без ролей
            }

            // Генерируем JWT-токен, передавая пользователя и роли
            return jwtHelper.GenerateToken(user, roles);
        }

    }
}
