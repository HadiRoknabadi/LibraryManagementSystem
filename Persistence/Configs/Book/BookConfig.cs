using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configs.Book
{
    public class BookConfig : IEntityTypeConfiguration<Domain.Entities.Book.Book>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Book.Book> builder)
        {
            builder.Property(b => b.Title).IsRequired().HasMaxLength(300);

            builder.Property(b => b.ISBN).IsRequired().HasMaxLength(20);

            builder.HasQueryFilter(b => b.IsDelete == false);
        }
    }
}
