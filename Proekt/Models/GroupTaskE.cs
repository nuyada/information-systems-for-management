using System.ComponentModel.DataAnnotations;

namespace Proekt.Models
{
    public class GroupTaskE
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "обязательное")]
        public string Title { get; set; }
        [Required(ErrorMessage = "обязательное")]
        public string Description { get; set; }
        [Required(ErrorMessage = "обязательное")]
        public DateTime DueDate { get; set; }
        [Required(ErrorMessage = "обязательное")]
        public bool IsCompleted { get; set; }
        public int CreatedBy { get; set; } // Пользователь, создавший задачу
        [Required(ErrorMessage = "обязательное")]
        public int GroupId { get; set; } // Группа, к которой относится задача
        [Required(ErrorMessage = "обязательное")]
        public int? CategoryId { get; set; } // Категория задачи (опционально)
    }
}
