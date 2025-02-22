using System.ComponentModel.DataAnnotations;

namespace Proekt.Entites
{
    public class TaskE
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        [Required(ErrorMessage = "обязательное")]
        public string Title { get; set; }
        [Required(ErrorMessage = "обязательное")]
        public string Description { get; set; }
        [Required(ErrorMessage = "обязательное")]
        public DateTime DueDate { get; set; }
        [Required(ErrorMessage = "обязательное")]
        public bool IsCompleted { get; set; }
        [Required(ErrorMessage = "обязательное")]

        public int CategoryId { get; set; }

    }
}
