using Domain.Entities.Book;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configs.Book
{
    public class AuthorConfig : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.Property(u => u.Name).IsRequired().HasMaxLength(200);
            builder.Property(u => u.Family).IsRequired().HasMaxLength(200);
            builder.Ignore(u => u.FullName);
            builder.HasQueryFilter(u => u.IsDelete == false);
        }
    }
}
