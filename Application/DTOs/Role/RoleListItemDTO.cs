namespace Application.DTOs.Role
{
    public class RoleListItemDTO
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
    }

    public enum GetRolesResult
    {
        Success,
        RolesEmpty
    }
}