using System.ComponentModel.DataAnnotations;

namespace Proekt.Entites
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "обязательное")]
        public string Name { get; set; }
        [Required(ErrorMessage = "обязательное")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "обязательное")]
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "обязательное")]
        public string Email { get; set; }
        [Required(ErrorMessage = "обязательное")]
        public string Password { get; set; }
    }
}
