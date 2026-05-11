using Application.Services.Interfaces.Context;
using Application.Utils;
using Domain.Entities.Account;
using Domain.Entities.Book;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data;
using System.Reflection;

namespace Persistence.Context
{
    public class ApplicationDbContext: IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>, IDatabaseContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        #region Book

        public DbSet<Book> Books { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }



        #endregion


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var persistenceAssembly = typeof(ApplicationDbContext).Assembly;

            builder.AddRestrictDeleteBehaviorConvention();

            builder.RegisterEntityTypeConfiguration(persistenceAssembly);
        }


        public override int SaveChanges()
        {
            _cleanString();
            _applyDateRules();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            _cleanString();
            _applyDateRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _cleanString();
            _applyDateRules();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            _cleanString();
            _applyDateRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void _cleanString()
        {
            var changedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (var item in changedEntities)
            {
                if (item.Entity == null)
                    continue;

                var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

                foreach (var property in properties)
                {
                    var propName = property.Name;
                    var val = (string)property.GetValue(item.Entity, null);

                    if (val.HasValue())
                    {
                        var newVal = val.Fa2En().FixPersianChars();
                        if (newVal == val)
                            continue;
                        property.SetValue(item.Entity, newVal, null);
                    }
                }
            }
        }

        private void _applyDateRules()
        {
            var entries = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                if (entity == null)
                    continue;

                var type = entity.GetType();

                var createDateProp = type.GetProperty("CreateDate");
                var lastUpdateDateProp = type.GetProperty("LastUpdateDate");

                if (createDateProp != null && lastUpdateDateProp != null)
                {
                    if (entry.State == EntityState.Added)
                    {
                        createDateProp.SetValue(entity, DateTime.Now);
                        lastUpdateDateProp.SetValue(entity, DateTime.Now);
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        entry.Property("CreateDate").IsModified = false;

                        lastUpdateDateProp.SetValue(entity, DateTime.Now);
                    }
                }
            }
        }


    }
}
