using Domain.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configs.Account
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Name).IsRequired().HasMaxLength(200);
            builder.Property(u => u.Family).IsRequired().HasMaxLength(200);
            builder.Property(u => u.PhoneNumber).IsRequired().HasMaxLength(11);
            builder.Property(u => u.UserAvatar).HasMaxLength(100);
            builder.Ignore(u => u.FullName);
            builder.HasQueryFilter(u => u.IsDelete == false);

        }
    }
}
