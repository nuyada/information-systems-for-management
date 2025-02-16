namespace Proekt.Entites
{
    public class UserRole
    {
        public int UserId { get; set; }
        public Users User { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
