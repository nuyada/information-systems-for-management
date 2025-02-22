using System.ComponentModel.DataAnnotations;

namespace Proekt.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "Имя обязательное")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Фамилия обязательная")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Отчество обязательное")]
        public string MiddleName { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Пароль обязательный")]
        [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
        [MaxLength(20, ErrorMessage = "Пароль не должен превышать 20 символов")]
        public string PasswordHash { get; set; }
    }
}
