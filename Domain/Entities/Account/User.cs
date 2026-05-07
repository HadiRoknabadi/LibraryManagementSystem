using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Account
{
    public class User:IdentityUser<int>
    {
        #region Properties

        public string Name { get; set; }
        public string Family { get; set; }
        public string UserAvatar { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string FullName { get { return $"{Name} {Family}"; } }


        #endregion

        #region Relations

        public virtual ICollection<UserRole> UserRoles { get; set; }

        #endregion

    }
}
