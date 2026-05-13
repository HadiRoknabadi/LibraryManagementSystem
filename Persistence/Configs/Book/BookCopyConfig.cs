using Domain.Entities.Book;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configs.Book
{
    public class BookCopyConfig : IEntityTypeConfiguration<BookCopy>
    {
        public void Configure(EntityTypeBuilder<BookCopy> builder)
        {
            builder.Property(x => x.InventoryCode).IsRequired().HasMaxLength(50);

            builder.Property(x => x.ShelfLocation).HasMaxLength(100);

            builder.HasQueryFilter(b => b.IsDelete == false);
        }
    }
}
