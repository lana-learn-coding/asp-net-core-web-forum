using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DAL.Database;
using DAL.Models;
using DAL.Models.Auth;
using DAL.Models.Topic;
using SlugityLib;

namespace DAL
{
    // I use EF6 instead of EFCore because i need the TPC model (which is not supported by EFCore yet)
    public class ModelContext : DbContext
    {
        private static readonly Slugity Slugity = new();
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public ModelContext(string connection) : base(connection)
        {
            System.Data.Entity.Database.SetInitializer(new DatabaseInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().Map(m => m.MapInheritedProperties());
            modelBuilder.Entity<User>().Map(m => m.MapInheritedProperties());
            modelBuilder.Entity<Category>().Map(m => m.MapInheritedProperties());
            modelBuilder.Entity<Tag>().Map(m => m.MapInheritedProperties());
        }

        public override int SaveChanges()
        {
            SetProperties();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            SetProperties();
            return base.SaveChangesAsync(cancellationToken);
        }

        // Audit date and slug generation handle
        private void SetProperties()
        {
            foreach (var entity in ChangeTracker.Entries().Where(p => p.State == EntityState.Added))
            {
                if (entity.Entity is not IAuditable created) continue;
                created.CreatedAt = DateTime.Now;
                created.UpdatedAt = DateTime.Now;

                if (entity.Entity is not ISlugged slugged) continue;
                slugged.Slug = Slugity.GenerateSlug(slugged.RawSlug);
            }

            foreach (var entity in ChangeTracker.Entries().Where(p => p.State == EntityState.Modified))
            {
                if (entity.Entity is not IAuditable updated) continue;
                updated.UpdatedAt = DateTime.Now;

                if (entity.Entity is not ISlugged slugged) continue;
                slugged.Slug = Slugity.GenerateSlug(slugged.RawSlug);
            }
        }
    }
}