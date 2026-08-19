namespace MusicPortal.BusinessLogic.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<RoleDTO> Roles { get; set; } = new();
    }
}