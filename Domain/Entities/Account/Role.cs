using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Account
{
    public class Role : IdentityRole<int>
    {
        #region Properties

        public bool IsDelete { get; set; }

        #endregion

        #region Relations

        public virtual ICollection<UserRole> UserRoles { get; set; }

        #endregion
    }
}
