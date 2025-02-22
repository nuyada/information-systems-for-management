using System.ComponentModel.DataAnnotations;

namespace Proekt.Models
{
    public class GroupE
    {
        [Required(ErrorMessage = "обязательное")]
        public int Id { get; set; }
        [Required(ErrorMessage = "обязательное")]
        public string Name { get; set; }
        [Required(ErrorMessage = "обязательное")]
        public string Description { get; set; }
        public int CreatedBy { get; set; } // Пользователь, создавший группу
    }
}
