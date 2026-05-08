namespace Application.DTOs.User
{
    public class UserListItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string FullName { get { return $"{Name} {Family}"; } }

        public string PhoneNumber { get; set; }
    }
}
