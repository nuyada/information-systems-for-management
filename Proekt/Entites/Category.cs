namespace Proekt.Entites
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TaskE> Tasks { get; set; } = new List<TaskE>();
    }
}
