using Microsoft.IdentityModel.Tokens;
using Proekt.Entites;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

namespace Proekt.Helpers
{
    public class JwtHelper
    {
        private const int TokenLifetimeMinutes = 60;
        private readonly IConfiguration _configuration;
        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public  string GenerateToken(Users user, List<string> roles)
        {
            // Создаем список клеймов
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // User ID
        new Claim(ClaimTypes.Email, user.Email)                  // Email пользователя
    };

            // Добавляем роли пользователя как отдельные клеймы
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Настройка ключа и подписи
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Создание токена
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(TokenLifetimeMinutes),
                signingCredentials: creds
            );

            // Генерация строки токена
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
