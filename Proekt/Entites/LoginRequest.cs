using System.ComponentModel.DataAnnotations;

namespace Proekt.Entites
{
    public class LoginRequest
    {
        public string Email { get; set; }
        [Required(ErrorMessage = "Пароль обязательный")]
        [MinLength(6,ErrorMessage ="Пароль должен содержать минимум 6 символов")]
        [MaxLength(20,ErrorMessage ="Пароль не должен превышать 20 символов")]
        public string Password { get; set; }
    }
}
