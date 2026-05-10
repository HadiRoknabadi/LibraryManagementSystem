using Domain.Entities.Book;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Services.Interfaces.Context
{
    public interface IDatabaseContext
    {

        #region Book

        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Author> Authors { get; set; }

        #endregion

        int SaveChanges();

        int SaveChanges(bool acceptAllChangesOnSuccess);

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        EntityEntry Entry(object entity);
    }
}
