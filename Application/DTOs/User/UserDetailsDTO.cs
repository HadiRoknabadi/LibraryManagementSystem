namespace Application.DTOs.User
{
    public class UserDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get { return $"{Name} {Family}"; } }
        public string UserAvatar { get; set; }

    }

    public enum GetUserDetailsResult
    {
        Success,
        UserNotFound
    }
}
