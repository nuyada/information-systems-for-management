using System.ComponentModel.DataAnnotations;

namespace Proekt.Entites
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "обязательное")]
        public string Name { get; set; }
        [Required(ErrorMessage = "обязательное")]
        public List<TaskE> Tasks { get; set; } = new List<TaskE>();
    }
}
