namespace Proekt.Models
{
    public class GroupTaskE
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int CreatedBy { get; set; } // Пользователь, создавший задачу
        public int GroupId { get; set; } // Группа, к которой относится задача
        public int? CategoryId { get; set; } // Категория задачи (опционально)
    }
}
