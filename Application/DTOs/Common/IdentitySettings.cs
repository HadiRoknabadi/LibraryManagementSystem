namespace Application.DTOs.Common
{
    public class IdentitySettings
    {
        public bool PasswordRequireDigit { get; set; }
        public int PasswordRequiredLength { get; set; }
        public bool PasswordRequireLowercase { get; set; }
        public bool PasswordRequireNonAlphanumeric { get; set; }
        public bool PasswordRequireUppercase { get; set; }
        public int PasswordRequiredUniqueChars { get; set; }
        public bool UserRequireUniqueEmail { get; set; }
        public int LockoutMaxFailedAccessAttempts { get; set; }
        public double LockoutDefaultLockoutTimeSpan { get; set; }
    }
}