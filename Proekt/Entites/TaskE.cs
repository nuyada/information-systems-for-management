namespace Proekt.Entites
{
    public class TaskE
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
       
        public int CategoryId { get; set; }

    }
}
