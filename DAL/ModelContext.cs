using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DAL.Database;
using DAL.Models;
using DAL.Models.Auth;
using DAL.Models.Forum;
using DAL.Models.Topic;
using SlugityLib;
using Thread = DAL.Models.Forum.Thread;

namespace DAL
{
    // I use EF6 instead of EFCore because i need the TPC model (which is not supported by EFCore yet)
    public class ModelContext : DbContext
    {
        private static readonly Slugity Slugity = new();

        public ModelContext(string connection) : base(connection)
        {
            System.Data.Entity.Database.SetInitializer(new FakeDataDatabaseInitializer());
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Forum> Forums { get; set; }

        public DbSet<Thread> Threads { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Role>().Map(m => m.MapInheritedProperties());
            modelBuilder.Entity<User>().Map(m => m.MapInheritedProperties());
            modelBuilder.Entity<Category>().Map(m => m.MapInheritedProperties());
            modelBuilder.Entity<Tag>().Map(m => m.MapInheritedProperties());

            modelBuilder.Entity<Vote>().Map(m => m.MapInheritedProperties());
            modelBuilder.Entity<Forum>().Map(m => m.MapInheritedProperties());
            modelBuilder.Entity<Post>().Map(m => m.MapInheritedProperties());
            modelBuilder.Entity<Thread>().Map(m => m.MapInheritedProperties());
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
                if (entity.Entity is IAuditable created)
                {
                    created.CreatedAt = DateTime.Now;
                    created.UpdatedAt = DateTime.Now;
                }


                if (entity.Entity is ITracked tracked)
                {
                    tracked.LastActivityAt = DateTime.Now;
                }

                if (entity.Entity is ISlugged slugged)
                {
                    slugged.Slug = Slugity.GenerateSlug(slugged.RawSlug);
                }
            }

            foreach (var entity in ChangeTracker.Entries().Where(p => p.State == EntityState.Modified))
            {
                var isNotTracking = true;

                if (entity.Entity is ITracked)
                {
                    var trackingProp = entity.Property("LastActivityAt");
                    isNotTracking = trackingProp.CurrentValue == trackingProp.OriginalValue;
                }

                if (entity.Entity is IAuditable created && isNotTracking)
                {
                    created.UpdatedAt = DateTime.Now;
                }


                if (entity.Entity is ISlugged slugged)
                {
                    slugged.Slug = Slugity.GenerateSlug(slugged.RawSlug);
                }
            }
        }
    }
}