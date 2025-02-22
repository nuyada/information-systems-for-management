using System.ComponentModel.DataAnnotations;

namespace Proekt.Entites
{
    public class Role
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "обязательное")]
        public string Name { get; set; }
    }
}
